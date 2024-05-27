using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FoodInteractable : MonoBehaviour, IInteract, IAdd
{
    private Transform _popUpPos;

    [SerializeField] private int _stackSize = 1;

    [SerializeField] private GameObject[] _Ingredients;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    public bool Interact(bool isToAdd)
    {
        if (isToAdd)
        {
            _Ingredients[_stackSize].SetActive(true);
            _stackSize++;
            return true;
        }
        if (!isToAdd && _stackSize > 0)
        {
            _stackSize--;
            _Ingredients[_stackSize].SetActive(false);
            return true;
        }
        return false;
    }

    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
    public void Change(int quantity)
    {
        _stackSize += quantity;
    }
    public int quantity()
    {
        return _stackSize;
    }
    public bool CanAddFood()
    {
        if(_stackSize < 10)
            return true;
        return false;
    }
}
