using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

public class HeldFood : MonoBehaviour, IHeldFood, IHeld
{
    private bool _isCutted;
    public int _isCooked;
    Material _material;
    public int ID;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        ID = new Random().Next(0, 1000000000);
    }
    public void ICooked(int cookValue)
    {
        _isCooked = cookValue;
    }
    public bool IsCooked()
    {
        if(_isCooked == 1)
        {
            return true;
        }
        return false;
    }
    public void ICutted()
    {
        _isCutted = true;
    }
    public bool IsCutted()
    {
        return _isCutted;
    }
    public void IBasicMaterial()
    {
        GetComponent<Renderer>().material = _material;
    }
    public int MyId()
    {
        return ID;
    }
}
