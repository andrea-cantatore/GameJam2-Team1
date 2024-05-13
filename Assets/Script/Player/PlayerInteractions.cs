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
    [SerializeField] Material _normalMaterial, _cookedMaterial, _overCookedMaterial;
    

    private void Awake()
    {
        _cam = Camera.main.transform;
    }
    
    private void OnEnable()
    {
        EventManager.OnGrillPickUp += GrillPickUp;
    }
    
    private void OnDisable()
    {
        EventManager.OnGrillPickUp -= GrillPickUp;
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
                    if (!_isHandFull)
                    {
                        if (hit.transform.tag == "Grill")
                        {
                            interactable.Interact();
                            return;
                        }
                        foreach (GameObject obj in _interactables)
                        {
                            if (obj.tag == hit.transform.tag)
                            {
                                Debug.Log("holy " + obj.name);
                                obj.GetComponent<MeshRenderer>().material = _normalMaterial;
                                obj.SetActive(true);
                                _heldObject = obj;
                                _isHandFull = true;
                                return;
                            }
                        }
                    }
                    if (hit.transform.tag == "Grill")
                    {
                        GrillInteraction();
                        return;
                    }
                    if (hit.transform.tag == _heldObject.tag)
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

    private void GrillInteraction()
    {
        if (_isHandFull)
        {
            EventManager.OnGrillInteraction?.Invoke(_heldObject.tag);
            _heldObject.SetActive(false);
            _isHandFull = false;
        }
    }
    
    private void GrillPickUp(int state, String tag)
    {
        GameObject toChange = InteractableCicle(tag);
        if (toChange == null)
        {
            Debug.Log(tag + " not found");
            return;
        }
        switch (state)
        {
            case 0:
                toChange.GetComponent<MeshRenderer>().material = _normalMaterial;
                break;
            case 1:
                toChange.GetComponent<MeshRenderer>().material = _cookedMaterial;
                break;
            case 2:
                toChange.GetComponent<MeshRenderer>().material = _overCookedMaterial;
                break;
        }
    }
    
    private GameObject InteractableCicle(String tag)
    {
        foreach (GameObject obj in _interactables)
        {
            if (obj.tag == tag)
            {
                Debug.Log("Picking up " + obj.name);
                obj.GetComponent<MeshRenderer>().material = _normalMaterial;
                obj.SetActive(true);
                _heldObject = obj;
                _isHandFull = true;
                return obj;
            }
        }
        return null;
    }
}
