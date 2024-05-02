using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Transform _cam;
    [SerializeField] private GameObject[] _interactables;
    [SerializeField] private float _interactionDistance = 5f;

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
                    foreach (GameObject obj in _interactables)
                    {
                        if(obj.tag == hit.transform.tag)
                        {
                            obj.SetActive(obj.activeSelf ? false : true);
                            return;
                        }
                    }
                }
            }
        }
    }
}
