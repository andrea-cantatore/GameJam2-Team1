using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManger : MonoBehaviour
{
    [SerializeField] private GameObject[] _customers;
    [SerializeField] private Transform[] _customerEndPos;
    [SerializeField] private bool[] _isEndPosFull;
    [SerializeField] private float _spawnRate;
    private bool _isNightStarting;
    private float _timer;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioData _audioData;


    private void Awake()
    {
        _timer = _spawnRate;
    }
    private void OnEnable()
    {
        EventManager.OnCustomerLeave += CustomerLeave;
        EventManager.IsNight += IsNight;
    }

    private void OnDisable()
    {
        EventManager.OnCustomerLeave -= CustomerLeave;
        EventManager.IsNight -= IsNight;
    }

    private void Update()
    {
        if (_isNightStarting)
        {
            if (CheckAllEndPos())
            {
                EventManager.StartNextNight?.Invoke();
            }
        }
        if (_isNightStarting)
            return;
        if (CheckEndPos())
        {
            _timer += Time.deltaTime;
            if (_timer >= _spawnRate)
            {
                SpawnCustomer();
            }
        }
    }

    /*private void SpawnCustomer()
    {

        if (CheckEndPos())
        {
            while (_timer >= _spawnRate)
            {
                int randomIndex = Random.Range(0, _customers.Length);
                int randomEndPos = Random.Range(0, _customerEndPos.Length);
                if (!_isEndPosFull[randomEndPos] && _customers[randomIndex].activeSelf == false)
                {
                    _customers[randomIndex].SetActive(true);
                    if (_customers[randomIndex].TryGetComponent(out ICustomer customer))
                    {
                        Debug.Log("Customer spawned");
                        _animator.SetTrigger("NewCustomer");
                        customer.GetTargetPos(_customerEndPos[randomEndPos]);
                    }
                    _isEndPosFull[randomEndPos] = true;

                    ResetTimer();
                }
            }
        }
    }*/

    //ByEmaStart
    private void SpawnCustomer()
    {
        if (_timer < _spawnRate || !CheckEndPos())
            return;

        bool customerSpawned = false;

        if (DayNightCicle.Instance.DayCount == 0)
        {
            for (int attempt = 0; attempt < _customers.Length * _customerEndPos.Length; attempt++)
            {
                int randomIndex = Random.Range(0, _customers.Length - 2);
                int randomEndPos = Random.Range(0, _customerEndPos.Length - 2);

                if (!_isEndPosFull[randomEndPos] && !_customers[randomIndex].activeSelf)
                {
                    _customers[randomIndex].SetActive(true);
                    if (_customers[randomIndex].TryGetComponent(out ICustomer customer))
                    {
                        AudioManager.instance.PlaySFX(_audioData.BellRing, transform);
                        _animator.SetTrigger("NewCustomer");
                        customer.GetTargetPos(_customerEndPos[randomEndPos]);
                    }
                    _isEndPosFull[randomEndPos] = true;
                    customerSpawned = true;
                    break;
                }
            }
        }
        if (DayNightCicle.Instance.DayCount == 1)
        {
            for (int attempt = 0; attempt < _customers.Length * _customerEndPos.Length; attempt++)
            {
                int randomIndex = Random.Range(0, _customers.Length - 1);
                int randomEndPos = Random.Range(0, _customerEndPos.Length - 1);

                if (!_isEndPosFull[randomEndPos] && !_customers[randomIndex].activeSelf)
                {
                    _customers[randomIndex].SetActive(true);
                    if (_customers[randomIndex].TryGetComponent(out ICustomer customer))
                    {
                        AudioManager.instance.PlaySFX(_audioData.BellRing, transform);
                        _animator.SetTrigger("NewCustomer");
                        customer.GetTargetPos(_customerEndPos[randomEndPos]);
                    }
                    _isEndPosFull[randomEndPos] = true;
                    customerSpawned = true;
                    break;
                }
            }
        }
        if (DayNightCicle.Instance.DayCount == 3)
        {
            for (int attempt = 0; attempt < _customers.Length * _customerEndPos.Length; attempt++)
            {
                int randomIndex = Random.Range(0, _customers.Length);
                int randomEndPos = Random.Range(0, _customerEndPos.Length);

                if (!_isEndPosFull[randomEndPos] && !_customers[randomIndex].activeSelf)
                {
                    _customers[randomIndex].SetActive(true);
                    if (_customers[randomIndex].TryGetComponent(out ICustomer customer))
                    {
                        AudioManager.instance.PlaySFX(_audioData.BellRing, transform);
                        _animator.SetTrigger("NewCustomer");
                        customer.GetTargetPos(_customerEndPos[randomEndPos]);
                    }
                    _isEndPosFull[randomEndPos] = true;
                    customerSpawned = true;
                    break;
                }
            }
        }


        if(customerSpawned)
        {
            ResetTimer();
        }

    }
    //ByEmaEnd

    private void IsNight(bool isNight)
    {
        _isNightStarting = isNight;
        ResetTimer();
    }

    private void ResetTimer()
    {
        _timer = 0;
    }

    private bool CheckEndPos()
    {
        if (DayNightCicle.Instance.DayCount == 0)
        {
            for (int i = 0; i < _isEndPosFull.Length - 2; i++)
            {
                if (!_isEndPosFull[i])
                {
                    return true;
                }
            }
            return false;
        }
        if (DayNightCicle.Instance.DayCount == 1)
        {
            for (int i = 0; i < _isEndPosFull.Length - 1; i++)
            {
                if (!_isEndPosFull[i])
                {
                    return true;
                }
            }
            return false;
        }
        for (int i = 0; i < _isEndPosFull.Length - 1; i++)
        {
            if (!_isEndPosFull[i])
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckAllEndPos()
    {
        for (int i = 0; i < _isEndPosFull.Length; i++)
        {
            if (_isEndPosFull[i])
            {
                return false;
            }
        }
        return true;
    }

    private void CustomerLeave(Transform customer)
    {
        for (int i = 0; i < _customerEndPos.Length; i++)
        {
            if (_customerEndPos[i] == customer)
            {
                _isEndPosFull[i] = false;
            }
        }
    }

}
