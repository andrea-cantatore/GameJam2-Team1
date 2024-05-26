using System;
using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Customer : MonoBehaviour, IInteract, ICustomer
{

    private Transform _popUpPos;
    [SerializeField] private DSDialogueContainerSO _dialogueContainer;
    [SerializeField] GameObject[] _expectedMeal;
    [SerializeField] private int _payment;
    [SerializeField] private PlayerInteractions _playerInteractions;
    [SerializeField] private float _permanenceTime, _permanenceTimeChanger;
    [SerializeField] private int _baseTip;
    [SerializeField] private GameObject[] _coins;
    private float _permanenceTimer;
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
    }

    private void OnEnable()
    {
        _expectedMealCounter = _expectedMeal.Length;
        _canMove = false;
        _alreadySpoken = false;
        _isOnLastPos = false;
        _pathIndex = 0;
        foreach (GameObject obj in _expectedMeal)
        {
            obj.SetActive(true);
        }
    }

    private void Update()
    {
        _permanenceTimer += Time.deltaTime;
        if (_permanenceTimer >= _permanenceTime)
        {
            _expectedMealCounter = 0;
        }
        if (_canMove && _expectedMealCounter > 0)
        {
            MoveToTarget();
        }
        if (_expectedMealCounter <= 0)
        {
            MoveBack();
        }
    }

    public bool Interact(bool isToAdd)
    {
        if (_expectedMealCounter <= 0)
        {
            MoveBack();
            return false;
        }
        if (!_alreadySpoken)
        {
            EventManager.OnStartingDialogue?.Invoke(_dialogueContainer, gameObject.name, this);
            _alreadySpoken = true;
        }
        else
        {
            foreach (GameObject obj in _expectedMeal)
            {
                if (_playerInteractions.HeldObject.tag == obj.tag)
                {
                    if (obj.TryGetComponent(out IDish dish) && obj.activeSelf)
                    {
                        Debug.Log("player food: " + _playerInteractions.ActiveFood());
                        Debug.Log("customer food: " + dish.ActiveFood());
                        Debug.Log(_playerInteractions.ActiveFood().SequenceEqual(dish.ActiveFood()));
                        if (_playerInteractions.ActiveFood().SequenceEqual(dish.ActiveFood()))
                        {
                            obj.SetActive(false);
                            _expectedMealCounter--;
                        }
                    }
                }
            }
        }
        if (_expectedMealCounter <= 0)
        {
            int tip = _baseTip * Mathf.RoundToInt((_permanenceTime - _permanenceTimer) / 60);
            _coins[0].SetActive(true);
            _coins[1].SetActive(true);
            if (_coins[0].TryGetComponent(out ICoin coin))
            {
                coin.AddCoins(_payment);
            }
            if (_coins[1].TryGetComponent(out ICoin coin1))
            {
                coin1.AddCoins(tip);
            }
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
    public void GetTargetPos(Transform pos)
    {
        _targetPos = pos;
        if (pos.TryGetComponent(out CustomerTargetPos customerTargetPos))
        {
            _pathPos = customerTargetPos.GetOtherPos();
            _coins = customerTargetPos.Coins();
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
            transform.position =
                Vector3.MoveTowards(transform.position, _pathPos[_pathIndex].position, 5 * Time.deltaTime);
            transform.LookAt(_pathPos[_pathIndex].position);
            if (Vector3.Distance(transform.position, _pathPos[_pathIndex].position) < 0.1f)
            {
                _pathIndex++;
            }
        }
        else if (!_isOnLastPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos.position, 5 * Time.deltaTime);
            //transform.LookAt(_pathPos[_pathIndex].position);
            if (Vector3.Distance(transform.position, _targetPos.position) < 0.1f)
            {
                _isOnLastPos = true;
                transform.LookAt(_targetPos.position + new Vector3(0, 0, -1));
            }
        }
        else
        {
            _canMove = false;
        }
    }

    private void MoveBack()
    {
        if (_pathIndex < _pathPos.Length)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _pathPos[_pathIndex].position, 5 * Time.deltaTime);
            transform.LookAt(_pathPos[_pathIndex].position);
            if (Vector3.Distance(transform.position, _pathPos[_pathIndex].position) < 0.1f)
            {
                _pathIndex--;
            }
            if (_pathIndex == 0)
            {
                EventManager.OnCustomerLeave?.Invoke(_targetPos);
                gameObject.SetActive(false);
            }
        }
        else
        {
            _pathIndex = _pathPos.Length - 1;
        }
    }

    public void ChangeHappines(int value)
    {
        if (value > 0)
        {
            _permanenceTime += _permanenceTimeChanger;
        }
        else
        {
            _permanenceTime -= _permanenceTimeChanger;
        }
    }
}
