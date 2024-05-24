using System;
using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;

public class Customer : MonoBehaviour, IInteract, ICustomer
{
    
    private Transform _popUpPos;
    [SerializeField] private DSDialogueContainerSO _dialogueContainer;
    [SerializeField] GameObject[] _expectedMeal;
    [SerializeField] private int _payment;
    [SerializeField] private PlayerInteractions _playerInteractions;
    private int _expectedMealCounter;
    private Transform _targetPos;
    private Transform[] _pathPos;
    private int _pathIndex;
    private bool _canMove;
    private bool _isOnLastPos;
    private bool _alreadySpoken;
    
    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
        _expectedMealCounter = _expectedMeal.Length;
    }

    private void OnEnable()
    {
        _canMove = false;
        _alreadySpoken = false;
        _isOnLastPos = false;
        _pathIndex = 0;
    }

    private void Update()
    {
        if(_canMove && _expectedMealCounter > 0)
        {
            MoveToTarget();
        }
        if (_expectedMealCounter == 0)
        {
            MoveBack();
        }
    }

    public bool Interact(bool isToAdd)
    {
        if (!_alreadySpoken)
        {
            EventManager.OnStartingDialogue?.Invoke(_dialogueContainer, gameObject.name);
            _alreadySpoken = true;
        }
        else
        {
            foreach (GameObject obj in _expectedMeal)
            {
                if (_playerInteractions.HeldObject == obj)
                {
                    _expectedMealCounter--;
                }
            }
            if(_expectedMealCounter == 0)
            {
                EventManager.OnFullBeer?.Invoke(_payment);
                gameObject.SetActive(false);
            }
        }
            
        return true;
    }

    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
    public void GetTargetPos(Transform pos)
    {
        _targetPos = pos;
        if (pos.TryGetComponent(out CustomerTargetPos customerTargetPos))
        {
            _pathPos = customerTargetPos.GetOtherPos();
            StartAction();
        }
    }
    public void StartAction()
    {
        _canMove = true;
    }
    
    private void MoveToTarget()
    {
        if (_pathIndex < _pathPos.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, _pathPos[_pathIndex].position, 5 * Time.deltaTime);
            if (Vector3.Distance(transform.position, _pathPos[_pathIndex].position) < 0.1f)
            {
                _pathIndex++;
            }
        }
        else if(!_isOnLastPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos.position, 5 * Time.deltaTime);
            if (Vector3.Distance(transform.position, _targetPos.position) < 0.1f)
            {
                _isOnLastPos = true;
                
            }
        }
        else
        {
            _canMove = false;
        }
    }
    
    private void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, _pathPos[_pathIndex].position, 5 * Time.deltaTime);
        if (Vector3.Distance(transform.position, _pathPos[_pathIndex].position) < 0.1f)
        {
            _pathIndex--;
        }
        if(_pathIndex == 0)
        {
            gameObject.SetActive(false);
        }
    }
}

