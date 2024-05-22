using System;
using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioData _audioData;
    private AudioClip _walkSound;
    [SerializeField] private float _moveVelocity;
    [SerializeField] private float _jumpVelocity;
    private Rigidbody _rb;
    private bool _isMovementUnlocked = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void OnEnable()
    {
        EventManager.OnCuttingInteraction += CanMove;
        EventManager.OnStartingDialogue += StartingDialogue;
        EventManager.OnDialogueEnd += () => _isMovementUnlocked = false;
    }
    private void OnDisable()
    {
        EventManager.OnCuttingInteraction -= CanMove;
        EventManager.OnStartingDialogue += StartingDialogue;
        EventManager.OnDialogueEnd += () => _isMovementUnlocked = false;
    }

    private void Update()
    {
        if(_isMovementUnlocked)
            Move();
    }
    
    private void Move()
    {
        Vector2 movement = InputManager.actionMap.PlayerInput.Movement.ReadValue<Vector2>();
        Vector3 direction = (transform.right * movement.x + transform.forward * movement.y).normalized;
        _rb.velocity = new Vector3(direction.x * _moveVelocity, _rb.velocity.y, direction.z * _moveVelocity);
    }
    private void CanMove(bool cantMove)
    {
        _isMovementUnlocked = !cantMove;
    }
    private void StartingDialogue(DSDialogueContainerSO containerSo, String dialogue)
    {
        _isMovementUnlocked = !_isMovementUnlocked;
    }
    
}
