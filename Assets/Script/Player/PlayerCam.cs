using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    
    [SerializeField] private Transform _playerOrientation;
    
    private float _xRotation;
    private float _yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void LateUpdate()
    {
        float mouseY = InputManager.actionMap.PlayerInput.Camera.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;
        float mouseX = InputManager.actionMap.PlayerInput.Camera.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        
        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _playerOrientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
