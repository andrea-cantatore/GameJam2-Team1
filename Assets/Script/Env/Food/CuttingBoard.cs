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
    bool isCutting;
    [SerializeField] private int _requiredCutting = 3;
    private int _cuttingCounter = 0;
    public bool IsCuttingEmpty = true;
    [SerializeField] private Animator _animator; // ByEma
    [SerializeField] private float _cutDelay = 1f; // ByEma
    private bool _canCut = true; // ByEma

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
        _mainCamera = Camera.main;
    }
    private void OnEnable()
    {
        EventManager.OnCuttingInteraction += CuttingInteraction;
        InputManager.actionMap.PlayerInput.Interaction.started += ctx => Interaction();
        InputManager.actionMap.PlayerInput.CookInteraction.started += ctx => InputCut();
    }

    private void OnDisable()
    {
        EventManager.OnCuttingInteraction -= CuttingInteraction;
    }

    /*private void Interaction()
    {
        if(isCutting)
            Interact(false);
    }

    private void InputCut()
    {
        if (isCutting)
        {
            _cuttingCounter++;
            if (_requiredCutting <= _cuttingCounter)
            {
                EventManager.OnCutted?.Invoke(_activeFood.tag, _activeFood.GetComponent<MeshRenderer>().material);
                _activeFood.SetActive(false);
                _activeFood = null;
                Interact(false);
                IsCuttingEmpty = true;
                _cuttingCounter = 0;
            }
        }
    }*/


    private void Interaction()
    {
        if (isCutting)
            Interact(false);
    }

    private void InputCut()
    {
        if (isCutting && _canCut)
        {
            StartCoroutine(CutCoroutine());
        }
    }

    private IEnumerator CutCoroutine()
    {
        _canCut = false;
        _animator.SetTrigger("isCutting");

        _cuttingCounter++;
        if (_requiredCutting <= _cuttingCounter)
        {
            yield return new WaitForSeconds(1f); //ByEma

            InvokingEvent();
            _activeFood.SetActive(false);
            _activeFood = null;
            Interact(false);
            IsCuttingEmpty = true;
            _cuttingCounter = 0;
            _animator.SetTrigger("isStopping");
        }

        yield return new WaitForSeconds(_cutDelay);
        _canCut = true;
    }


    public bool Interact(bool isToAdd)
    {
        isCutting = isToAdd;
        EventManager.OnCuttingInteraction?.Invoke(isCutting);


        return true;
    }

    public void InteractionPopUp()
    {
        if (isCutting)
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

    public bool CutInteraction(GameObject gameObject)
    {
        foreach (GameObject obj in _food)
        {
            if (obj.tag == gameObject.tag)
            {
                if (gameObject.TryGetComponent(out IHeldFood heldFood) && !heldFood.IsCooked())
                {
                    if (!obj.CompareTag("TomatoPick") && !obj.CompareTag("VegetablePick"))
                    {
                        return false;
                    }
                }
                obj.SetActive(true);
                IsCuttingEmpty = false;
                _activeFood = obj;
                return true;
            }
        }
        return false;
    }

    private void InvokingEvent()
    {
        if (_activeFood.tag == "FishPick")
            EventManager.OnCutted?.Invoke("FishSliced");
        if (_activeFood.tag == "SteakPick")
            EventManager.OnCutted?.Invoke("SteakSliced");
        if (_activeFood.tag == "ChickenPick")
            EventManager.OnCutted?.Invoke("ChickenSliced");
        if (_activeFood.tag == "TomatoPick")
            EventManager.OnCutted?.Invoke("TomatoSliced");
        if (_activeFood.tag == "PotatoPick")
            EventManager.OnCutted?.Invoke("PotatoSliced");
        if (_activeFood.tag == "VegetablePick")
            EventManager.OnCutted?.Invoke("VegetableSliced");
    }

}
