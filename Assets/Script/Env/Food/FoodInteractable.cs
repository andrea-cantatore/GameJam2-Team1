using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInteractable : MonoBehaviour, IInteract
{
    private Transform _popUpPos;

    private void Awake()
    { 
        _popUpPos = transform.GetChild(0);
    }

    public void Interact()
    {
        Debug.Log("Interacted with food");
    }

    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionText.transform.position = _popUpPos.position;
    }
}

