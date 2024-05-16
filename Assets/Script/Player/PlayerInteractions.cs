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
                            interactable.Interact(false);
                            return;
                        }
                        foreach (GameObject obj in _interactables)
                        {
                            if (obj.tag == hit.transform.tag)
                            {
                                if (interactable.Interact(false))
                                {
                                    obj.SetActive(true);
                                    _heldObject = obj;
                                    _isHandFull = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (hit.transform.tag == "Grill")
                    {
                        GrillInteraction(hit.transform.gameObject);
                        return;
                    }
                    if (hit.transform.tag == _heldObject.tag)
                    {
                        interactable.Interact(true);
                        _heldObject.SetActive(false);
                        _heldObject = null;
                        _isHandFull = false;
                        return;
                    }
                    if (hit.transform.tag == "CuttingBoard")
                    {
                        interactable.Interact(true);
                        return;
                    }

                    Debug.Log("You can't hold more than one object at a time");
                }
            }
            if (hit.transform.TryGetComponent(out IStash stash))
            {
                stash.InteractionPopUp();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (_heldObject != null)
                    {
                        _isHandFull = false;
                        _heldObject.SetActive(false);
                        _heldObject = null;
                    }
                }
            }
        }
    }

    private void GrillInteraction(GameObject obj)
    {
        if (_isHandFull)
        {
            obj.TryGetComponent(out IGrill grill);
            if (grill.GrillInteraction(_heldObject.tag, _heldObject.GetComponent<MeshRenderer>().material))
            {
                _heldObject.SetActive(false);
                _heldObject = null;
                _isHandFull = false;
            }
        }
    }

    private void GrillPickUp(int state, String tag, Material material)
    {
        GameObject toChange = InteractableCicle(tag, material);
        if (toChange == null)
        {
            Debug.Log(tag + " not found");
        }
    }

    private GameObject InteractableCicle(String tag, Material material)
    {
        foreach (GameObject obj in _interactables)
        {
            if (obj.tag == tag)
            {
                Debug.Log("Picking up " + obj.name);
                obj.GetComponent<MeshRenderer>().material = material;
                obj.SetActive(true);
                _heldObject = obj;
                _isHandFull = true;
                return obj;
            }
        }
        return null;
    }
}
