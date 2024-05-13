using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grill : MonoBehaviour, IInteract
{
    [SerializeField] private GameObject[] _food;
    private GameObject _grillingFood;
    private Transform _popUpPos;
    private bool _isGrilling = false, isCooked = false, isOverCooked = false;
    [SerializeField] private float _grillTime = 5f, overCookTime = 10f;
    private float _timer = 0f;
    [SerializeField] Material _normalMaterial, _cookedMaterial, _overCookedMaterial;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    private void OnEnable()
    {
        EventManager.OnGrillInteraction += GrillInteraction;
    }

    private void OnDisable()
    {
        EventManager.OnGrillInteraction -= GrillInteraction;
    }

    public void GrillInteraction(String tag)
    {
        foreach (GameObject obj in _food)
        {
            if (obj.tag == tag)
            {
                if (!_isGrilling)
                {
                    obj.GetComponent<MeshRenderer>().material = _normalMaterial;
                    _grillingFood = obj;
                    obj.SetActive(true);
                    _isGrilling = true;
                    StartCoroutine(GrillFood(obj));
                }
            }
        }
    }


    public void Interact()
    {
        _isGrilling = false;
        _timer = 0f;
        EventManager.OnGrillPickUp?.Invoke(isCooked ? 1 : isOverCooked ? 2 : 0, _grillingFood.tag);
        isCooked = false;
        isOverCooked = false;
        _grillingFood.SetActive(false);
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
            _timer += Time.deltaTime;
            if (_timer >= _grillTime && !isCooked)
            {
                food.GetComponent<MeshRenderer>().material = _cookedMaterial;
                isCooked = true;
            }
            if (_timer >= overCookTime && !isOverCooked)
            {
                food.GetComponent<MeshRenderer>().material = _overCookedMaterial;
                isOverCooked = true;
            }
            yield return null;
        }
    }
}
