using UnityEngine;

public class CustomerManger : MonoBehaviour
{
    [SerializeField] private GameObject[] _customers;
    [SerializeField] private Transform[] _customerEndPos;
    [SerializeField] private bool[] _isEndPosFull;
    [SerializeField] private float _spawnRate;
    private float _timer;

    private void Start()
    {
        SpawnCustomer();
    }
    private void Update()
    {
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
                Debug.Log(randomIndex+randomEndPos);
                if (!_isEndPosFull[randomEndPos] && _customers[randomIndex].activeSelf == false)
                {
                    _customers[randomIndex].SetActive(true);
                    if (_customers[randomIndex].TryGetComponent(out ICustomer customer))
                    {
                        
                    }
                    _isEndPosFull[randomEndPos] = true;
                    ResetTimer();
                }
            }
        }
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

}
