using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterHolder : MonoBehaviour, IInteract, ICounterHolder
{
    private bool _isFull, _isHoldingDish;
    private int _holdingID;
    private GameObject _holdingObject;
    [SerializeField] private Transform _holdingPosition, _popUpPos;
    private PlayerDish _playerDish;
    

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    public bool TakeObject(int id, GameObject obj, bool isDish)
    {
        if (_isHoldingDish)
        {
            Debug.Log("Dish is holding");
            return _playerDish.GetDish(obj, obj.GetComponent<HeldFood>().IsSliced);
        }
        Debug.Log(_isHoldingDish);
        Debug.Log("Dish is not holding");
        if(_isFull)
        {
            return false;
        }
        
        _holdingID = id;
        _isFull = true;
        _holdingObject = Instantiate(obj, _holdingPosition.position, Quaternion.identity);
        Debug.Log("Object Taken " + _holdingObject.name + _holdingID + _holdingPosition.position + "_______isDish " + isDish);
        if(isDish)
        {
            _isHoldingDish = true;
            _playerDish = _holdingObject.GetComponent<PlayerDish>();
            Debug.Log(_playerDish.name);
        }
        return true;
    }
    public bool ReleaseObject(int id)
    {
        if(!_isFull)
            return false;
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
    public bool HaveDishOn()
    {
        return _isHoldingDish;
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
