using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour, IInteract, ICutting
{
    [SerializeField] private GameObject[] _food;
    private GameObject _activeFood;
    private Transform _popUpPos;
    [SerializeField] private Transform _cameraOnInteraction;
    private Camera _mainCamera;
    bool isCutting = false;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
        _mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        EventManager.OnCuttingInteraction += CuttingInteraction;
    }
    
    private void OnDisable()
    {
        EventManager.OnCuttingInteraction -= CuttingInteraction;
    }
    
    public bool Interact(bool isToAdd)
    {
        isCutting = !isCutting;
        EventManager.OnCuttingInteraction?.Invoke(isCutting);
        if (!isCutting)
        {
            _activeFood.SetActive(false);
            _activeFood = null;
        }
        
        return true;
    }

    public void InteractionPopUp()
    {
        if(isCutting)
            return;
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
    
    private void CuttingInteraction(bool isCutting)
    {
        if (isCutting)
        {
            _mainCamera.transform.position = _cameraOnInteraction.position;
            _mainCamera.transform.rotation = _cameraOnInteraction.rotation;
        }
    }

    public void CutInteraction(String tag)
    {
        foreach (GameObject obj in _food)
        {
            if (obj.tag == tag)
            {
                obj.SetActive(true);
                _activeFood = obj;
            }
        }
    }
}
