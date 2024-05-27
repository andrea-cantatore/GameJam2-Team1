using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInteract : MonoBehaviour
{
    private Vector3 _popUpStartPos;
    private float _timer;
    
    private void Awake()
    {
        _popUpStartPos = transform.position;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        
        if(transform.position != _popUpStartPos)
        {
            _timer += Time.deltaTime;
            if (_timer >= 0.5f)
            {
                transform.position = _popUpStartPos;
                _timer = 0;
            }
        }
    }
}
