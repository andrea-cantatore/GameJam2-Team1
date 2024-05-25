using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour, IInteract, ICauldron
{
    [SerializeField] private String[] _tags = { "PotatoSliced", "VegetableSliced", "TomatoSliced", "FishSliced" };
    private bool[] _isInside = { false, false, false, false };
    [SerializeField] private GameObject[] _foods;
    private float _timer = 0f;
    [SerializeField] private float _cookingTime = 60f;
    [SerializeField] private Animator _animator;
    private Transform _popUpPos;
    private bool _isCooking = false;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    private void Update()
    {
        foreach (bool b in _isInside)
        {
            if(!b)
                _isCooking = true;
            else
                _isCooking = false;
        }
        if (_isCooking)
        {
            if(_timer + 2 >= _cookingTime)
            {
                _animator.SetBool("IsCooking", true);
            }
            if(_timer >= _cookingTime)
            {
                foreach (GameObject food in _foods)
                {
                    food.SetActive(false);
                }
                _animator.SetBool("IsCooking", false);
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }
    }

    public bool Interact(bool isToAdd)
    {
        foreach (bool b in _isInside)
        {
            if(!b)
                return false;
        }
        return true;
    }
    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
    public bool TakeObject(string tag)
    {
        for (int i = 0; i < _tags.Length; i++)
        {
            if (_tags[i] == tag && !_isInside[i])
            {
                _isInside[i] = true;
                _foods[i].SetActive(true);
                _timer = 0f;
                return true;
            }
        }
        return false;
    }
    
}
