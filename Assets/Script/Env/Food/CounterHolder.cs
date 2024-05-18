using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHolder : MonoBehaviour, IInteract,ICounterHolder
{
    private bool _isFull;
    private int _holdingID;
    private GameObject _holdingObject;
    [SerializeField] private Transform _holdingPosition, _popUpPos;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    public bool TakeObject(int id, GameObject obj)
    {
        if(_isFull)
        {
            return false;
        }
        _holdingID = id;
        _isFull = true;
        _holdingObject = Instantiate(obj, _holdingPosition.position, Quaternion.identity);
        Debug.Log("Object Taken " + _holdingObject.name + _holdingID + _holdingPosition.position);
        return true;
    }
    public bool ReleaseObject(int id)
    {
        if(id == _holdingID)
        {
            return true;
        }
        return false;
    }
    public Material GetMaterial()
    {
        return _holdingObject.GetComponent<Renderer>().material;
    }
    public void DestroyObject()
    {
        Destroy(_holdingObject);
        _holdingObject = null;
        _isFull = false;
    }
    public bool Interact(bool isToAdd)
    {
        return true;
    }
    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
}
