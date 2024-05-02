using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private float _moveVelocity;
    [SerializeField] private float _jumpVelocity;
    private Rigidbody _rb;
    

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }
    
    private void Move()
    {
        Vector2 movement = InputManager.actionMap.PlayerInput.Movement.ReadValue<Vector2>();
        Vector3 direction = (transform.right * movement.x + transform.forward * movement.y).normalized;
        _rb.velocity = new Vector3(direction.x * _moveVelocity, _rb.velocity.y, direction.z * _moveVelocity);
    }
    
}
