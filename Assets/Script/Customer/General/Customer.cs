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
    [SerializeField] private GameObject _orderPopUp;
    private Transform _orderPopUpOriginPos;
    private int _boardIndex;
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
        _orderPopUpOriginPos = _orderPopUp.transform;
    }

    private void OnEnable()
    {
        _boardIndex = 100;
        _expectedMealCounter = _expectedMeal.Length;
        _canMove = false;
        _alreadySpoken = false;
        _isOnLastPos = false;
        _pathIndex = 0;
        foreach (GameObject obj in _expectedMeal)
        {
            obj.SetActive(true);
        }
        EventManager.BoardIntSaver += SetBoardIndex;
    }

    private void OnDisable()
    {
        EventManager.BoardIntSaver -= SetBoardIndex;
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
        bool something = false;
        if (_expectedMealCounter <= 0)
        {
            MoveBack();
            return false;
        }
        if (!_alreadySpoken)
        {
            _alreadySpoken = true;
            EventManager.OnStartingDialogue?.Invoke(_dialogueContainer, gameObject.name, this);
            EventManager.OnStartingDialogue?.Invoke(_dialogueContainer, gameObject.name, this);
            EventManager.OnOrder?.Invoke(_orderPopUp);
            return false;
        }
        else
        {
            
            foreach (GameObject obj in _expectedMeal)
            {
                if (!obj.activeSelf)
                    continue;
                if (_playerInteractions.HeldObject.tag == obj.tag)
                {
                    Debug.Log("checking " + obj.tag + " " + _playerInteractions.HeldObject.tag);
                    if (obj.TryGetComponent(out IDish dish) && obj.activeSelf)
                    {
                        Debug.Log(_playerInteractions.ActiveFood() + " " + dish.IDReturner());
                        if (_playerInteractions.ActiveFood() == dish.IDReturner())
                        {
                            obj.SetActive(false);
                            _expectedMealCounter--;
                            something = true;
                            break;
                        }
                    }
                    else if (obj.TryGetComponent(out IMug mug))
                    {
                        Debug.Log(_playerInteractions.ActiveFood() + " " + mug.IDReturner());
                        if (_playerInteractions.ActiveFood() == mug.IDReturner())
                        {
                            obj.SetActive(false);
                            _expectedMealCounter--;
                            something = true;
                            break;
                        }
                    }
                    else if (obj.TryGetComponent(out IBowl bowl))
                    {
                        Debug.Log(_playerInteractions.ActiveFood() + " " + bowl.IDReturner());
                        if (_playerInteractions.ActiveBowl() == bowl.IDReturner())
                        {
                            obj.SetActive(false);
                            _expectedMealCounter--;
                            something = true;
                            break;
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
            return true;
        }
        return something;
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
                EventManager.BoardIntClearer?.Invoke(_boardIndex);
                _orderPopUp.transform.position = new Vector3(0, 0, 0);
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

    public void SetBoardIndex(int index)
    {
        if (_alreadySpoken && _boardIndex == 100)
            _boardIndex = index;
    }
}
