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
    private Transform _targetPos;
    private Transform[] _pathPos;
    private int _pathIndex;
    private bool _canMove;
    private bool _isOnLastPos;
    private bool _alreadySpoken;
    
    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
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
        if(_canMove)
        {
            MoveToTarget();
        }
    }

    public bool Interact(bool isToAdd)
    {
        if(_alreadySpoken)
            return false;
        EventManager.OnStartingDialogue?.Invoke(_dialogueContainer, gameObject.name);
        _alreadySpoken = true;
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
}

