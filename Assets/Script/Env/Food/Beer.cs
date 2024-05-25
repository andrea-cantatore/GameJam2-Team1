using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Beer : MonoBehaviour, IInteract
{

    [SerializeField] private float _holdingTime = 5f;
    [SerializeField] private GameObject _beer;
    [SerializeField] private Transform _popUpPos;
    [SerializeField] private int _beerType;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _spillAmount;
    private bool _isUsing;
    private float _timer;
    
    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    private void Update()
    {
        if (_isUsing)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                _animator.SetBool("Spill", true);
                _timer += Time.deltaTime;
                if(_timer >= _holdingTime)
                {
                    _isUsing = false;
                    _timer = 0;
                    EventManager.OnBeerInteraction?.Invoke(false);
                    _beer.SetActive(false);
                    EventManager.OnFullBeer?.Invoke(_beerType);
                    _animator.SetBool("Spill", false);
                    _spillAmount--;
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _animator.SetBool("Spill", false);
            }
        }
    }

    public bool Interact(bool isToAdd)
    {
        if(_spillAmount <= 0)
            return false;
        EventManager.OnBeerInteraction?.Invoke(true);
        _isUsing = true;
        _beer.SetActive(true);
        return true;
    }
    public void InteractionPopUp()
    {
        if(_isUsing)
            return;
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
}
