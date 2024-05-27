using System;
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

    private void Start()
    {
        SpawnCustomer();
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
        if(_isNightStarting) 
            return;
        _timer += Time.deltaTime;
        if (_timer >= _spawnRate)
        {
            SpawnCustomer();
        }
    }

    private void SpawnCustomer()
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
                        customer.GetTargetPos(_customerEndPos[randomEndPos]);
                    }
                    _isEndPosFull[randomEndPos] = true;
                    _animator.SetTrigger("NewCustomer");
                    
                    ResetTimer();
                }
            }
        }
    }
    
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
        for (int i = 0; i < _isEndPosFull.Length; i++)
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
