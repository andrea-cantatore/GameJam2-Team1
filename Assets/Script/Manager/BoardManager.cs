using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform[] _orderPositions;
    private bool[] _isOccupied;


    private void OnEnable()
    {
        EventManager.OnOrder += PullOnPosition;
        EventManager.BoardIntClearer += ClearPosition;
    }
    
    private void OnDisable()
    {
        EventManager.OnOrder -= PullOnPosition;
        EventManager.BoardIntClearer -= ClearPosition;
    }
    
    private void Awake()
    {
        _isOccupied = new bool[_orderPositions.Length];
    }
    
    private void PullOnPosition(GameObject obj)
    {
        for (int i = 0; i < _isOccupied.Length; i++)
        {
            if (_isOccupied[i] == false)
            {
                obj.transform.position = _orderPositions[i].position;
                Debug.Log("Position: " + i);
                _isOccupied[i] = true;
                EventManager.BoardIntSaver?.Invoke(i);
                return;
            }
        }
    }
    
    private void ClearPosition(int index)
    {
        _isOccupied[index] = false;
        foreach (bool b in _isOccupied)
        {
            Debug.Log(b);
        }
    }
}
