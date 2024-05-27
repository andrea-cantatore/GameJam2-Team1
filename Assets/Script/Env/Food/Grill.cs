using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grill : MonoBehaviour, IInteract, IGrill
{
    [SerializeField] private GameObject[] _food;
    private GameObject _grillingFood;
    private Transform _popUpPos;
    private bool _isGrilling = false, isCooked = false, isOverCooked = false;
    [SerializeField] private float _grillTime = 5f, overCookTime = 10f;
    private float _timer = 0f;
    [SerializeField] Material[] _steakMaterials, _potatoMaterials, _chickenMaterials, _fishMaterials;

    [SerializeField] private Animator _animator; // ByEma

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }


    public bool GrillInteraction(String tag, Material _currentMaterial)
    {
        if (_isGrilling)
        {
            return false;
        }
        foreach (GameObject obj in _food)
        {
            if (obj.tag == tag)
            {
                if (!_isGrilling)
                {
                    obj.GetComponent<MeshRenderer>().material = _currentMaterial;
                    _grillingFood = obj;
                    obj.SetActive(true);
                    _isGrilling = true;
                    if (_currentMaterial == _steakMaterials[1]
                        || _currentMaterial == _potatoMaterials[1]
                        || _currentMaterial == _chickenMaterials[1]
                        || _currentMaterial == _fishMaterials[1])
                    {
                        _timer = _grillTime;
                    }
                    else if (_currentMaterial == _steakMaterials[2]
                             || _currentMaterial == _potatoMaterials[2]
                             || _currentMaterial == _chickenMaterials[2]
                             || _currentMaterial == _fishMaterials[2])
                    {
                        _timer = overCookTime;
                    }

                    StartCoroutine(GrillFood(obj));
                    return true;
                }
            }
        }
        return false;
    }


    public bool Interact(bool isToAdd = true)
    {
        if (!_isGrilling)
        {
            return false;
        }
        _isGrilling = false;
        if (_timer >= _grillTime && _timer < overCookTime)
        {
            Debug.Log("Cooked");
            EventManager.OnGrillPickUp?.Invoke(1, _grillingFood.tag,
                _grillingFood.GetComponent<MeshRenderer>().material);
        }
        else if (_timer >= overCookTime)
        {
            EventManager.OnGrillPickUp?.Invoke(2, _grillingFood.tag,
                _grillingFood.GetComponent<MeshRenderer>().material);
        }
        else
        {
            EventManager.OnGrillPickUp?.Invoke(0, _grillingFood.tag,
                _grillingFood.GetComponent<MeshRenderer>().material);
        }
        _animator.SetTrigger("Empty");
        isCooked = false;
        isOverCooked = false;
        _timer = 0f;
        _grillingFood.SetActive(false);
        return true;
    }
    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }

    private IEnumerator GrillFood(GameObject food)
    {
        while (_isGrilling)
        {
            if (_timer >= _grillTime && !isCooked)
            {
                if (food.tag == "SteakPick")
                    food.GetComponent<MeshRenderer>().material = _steakMaterials[1];
                else if (food.tag == "PotatoPick")
                    food.GetComponent<MeshRenderer>().material = _potatoMaterials[1];
                else if (food.tag == "ChickenPick")
                    food.GetComponent<MeshRenderer>().material = _chickenMaterials[1];
                else if (food.tag == "FishPick")
                    food.GetComponent<MeshRenderer>().material = _fishMaterials[1];
                isCooked = true;
                _animator.SetTrigger("CookedAlarm");
            }
            if (_timer >= overCookTime && !isOverCooked)
            {
                if (food.tag == "SteakPick")
                    food.GetComponent<MeshRenderer>().material = _steakMaterials[2];
                else if (food.tag == "PotatoPick")
                    food.GetComponent<MeshRenderer>().material = _potatoMaterials[2];
                else if (food.tag == "ChickenPick")
                    food.GetComponent<MeshRenderer>().material = _chickenMaterials[2];
                else if (food.tag == "FishPick")
                    food.GetComponent<MeshRenderer>().material = _fishMaterials[2];
                isOverCooked = true;
                _animator.SetTrigger("BurnedAlarm");
            }
            _timer += Time.deltaTime;
            yield return null;
        }
    }
}
