using System;
using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private float _sensitivity;
    
    [SerializeField] private Transform _playerOrientation, _camPos;

    private bool _isMovementLocked;
    
    private float _xRotation;
    private float _yRotation;
    

    private void OnEnable()
    {
        EventManager.OnBeerInteraction += OnLockedCamera;
        EventManager.OnBookInteraction += OnLockedCamera;
        EventManager.OnCuttingInteraction += OnLockedCamera;
        EventManager.OnStartingDialogue += StartingDialogue;
        EventManager.OnDialogueEnd += () => _isMovementLocked = false;
    }
    private void OnDisable()
    {
        EventManager.OnBeerInteraction -= OnLockedCamera;
        EventManager.OnBookInteraction -= OnLockedCamera;
        EventManager.OnCuttingInteraction -= OnLockedCamera;
        EventManager.OnStartingDialogue -= StartingDialogue;
        EventManager.OnDialogueEnd -= () => _isMovementLocked = false;
    }

    private void LateUpdate()
    {
        if (_isMovementLocked)
        {
            return;
        }
        
        float mouseY = InputManager.actionMap.PlayerInput.Camera.ReadValue<Vector2>().y * _sensitivity * Time.deltaTime;
        float mouseX = InputManager.actionMap.PlayerInput.Camera.ReadValue<Vector2>().x * _sensitivity * Time.deltaTime;
        
        _yRotation += mouseX;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _playerOrientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
    
    private void StartingDialogue(DSDialogueContainerSO containerSo, String dialogue, Customer customer)
    {
        _isMovementLocked = !_isMovementLocked;
    }
    
    private void OnLockedCamera(bool isLocked)
    {
        _isMovementLocked = isLocked;
        if (!isLocked)
        {
            Camera.main.transform.position = _camPos.position;
        }
    }
    
}
