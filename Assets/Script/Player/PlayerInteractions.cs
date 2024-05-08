using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Transform _cam;
    [SerializeField] private GameObject[] _interactables;
    [SerializeField] private float _interactionDistance = 5f;
    private bool _isHandFull = false;
    private GameObject _heldObject;

    private void Awake()
    {
        _cam = Camera.main.transform;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.position, _cam.forward, out hit, _interactionDistance))
        {
            if (hit.transform.TryGetComponent(out IInteract interactable))
            {
                interactable.InteractionPopUp();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Interacting with " + hit.transform.name);
                    if(!_isHandFull)
                    {
                        foreach (GameObject obj in _interactables)
                        {
                            if(obj.tag == hit.transform.tag)
                            {
                                obj.SetActive(true);
                                _heldObject = obj;
                                _isHandFull = true;
                                return;
                            }
                        }   
                    }
                    if(hit.transform.tag == _heldObject.tag)
                    {
                        _heldObject.SetActive(false);
                        _isHandFull = false;
                    }
                    else
                    {
                        Debug.Log("You can't hold more than one object at a time");
                    }
                    
                }
            }
        }
    }
}
