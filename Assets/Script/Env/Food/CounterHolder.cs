using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CounterHolder : MonoBehaviour, IInteract, ICounterHolder
{
    private bool _isFull, _isHoldingDish;
    private int _holdingID;
    private GameObject _holdingObject;
    [SerializeField] private Transform _holdingPosition, _popUpPos;
    private PlayerDish _playerDish;
    private IBowl _bowl;
    

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    public bool TakeObject(int id, GameObject obj, bool isDish, bool isSliced, int _isCooked)
    {
        if (_isHoldingDish)
        {
            return _playerDish.GetDish(obj, obj.GetComponent<HeldFood>().IsSliced, _isCooked);
        }
        if(_isFull)
        {
            return false;
        }
        
        _holdingID = id;
        _isFull = true;
        _holdingObject = Instantiate(obj, _holdingPosition.position, Quaternion.identity);
        
        if(isDish)
        {
            _isHoldingDish = true;
            if(_holdingObject.TryGetComponent(out PlayerDish dish))
                _playerDish = dish;
            else if(_holdingObject.TryGetComponent(out IBowl bowl))
                _bowl = bowl;
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
        _isHoldingDish = false;
        _isFull = false;
    }
    
    public bool[] ReleaseDish(bool isDish)
    {
        if(isDish)
            return _playerDish.ReleaseDish();
        return _bowl.ReleaseDish();
    }
    public void DestroyDish()
    {
        Destroy(_holdingObject);
        _holdingObject = null;
        _isHoldingDish = false;
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
