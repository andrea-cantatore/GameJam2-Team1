using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class HeldFood : MonoBehaviour, IHeldFood, IHeld
{
    public bool IsSliced;
    Material _material;
    public int ID;
    public int _isCooked { get; set; }

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        ID = new Random().Next(0, 1000000000);
    }
    private void Update()
    {
        if (gameObject.tag == "PotatoSliced")
        {
            _isCooked = 1;
        }
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
        IsSliced = true;
    }
    public bool IsCutted()
    {
        return IsSliced;
    }
    public void IBasicMaterial()
    {
        if (gameObject.tag == "Mug")
        {
            return;
        }
        GetComponent<Renderer>().material = _material;
    }
    public int MyId()
    {
        return ID;
    }
}
