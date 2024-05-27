using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInteract : MonoBehaviour
{
    private Vector3 _popUpStartPos;

    private void OnEnable()
    {
        EventManager.ResetPopUp += ResetPos;
    }
    
    private void OnDisable()
    {
        EventManager.ResetPopUp -= ResetPos;
    }

    private void Awake()
    {
        _popUpStartPos = transform.position;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        
    }
    
    private void ResetPos()
    {
        transform.position = _popUpStartPos;
    }
}
