using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MerchantManager : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;

    [SerializeField] private string[] _foodTags =
    {
        "Steak", "Potato", "Vegetable", "Chicken",
        "Fish", "Tomato", "Amber", "Blonde", "Red"
    };

    private int _foodIndex;
    private bool _isPositive;

    [SerializeField] private GameObject[] _foods;

    [SerializeField] private GameObject[] _bars;
    [SerializeField] private TMP_Text _taxCounter;

    private int _moneyCounterInt;

    [SerializeField] private TMP_Text _costCounter;
    private int _costCounterInt;

    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private int[] _cost;

    [SerializeField] private TMP_Text _endMoney;

    private void OnEnable()
    {
        _moneyCounterInt = _uiManager.MoneyCounterInt;
        _moneyText.text = _moneyCounterInt.ToString();
        _costCounter.text = "0";
        _costCounterInt = 0;
        BarSetup();
        _taxCounter.text = "Taxes to pay: " + (DayNightCicle.Instance._tax * (DayNightCicle.Instance.DayCount + 1));
    }

    public void AddFood(int index)
    {
        if (_foods[index].TryGetComponent(out IAdd add))
        {
            if (_isPositive && CanAddFood(add))
            {
                if (_moneyCounterInt < _cost[index])
                {
                    return;
                }
                add.Change(1);
                _costCounterInt += _cost[index];
                _costCounter.text = "-" + Math.Abs(_costCounterInt);
                _bars[index].transform.GetChild(add.quantity()).gameObject.SetActive(true);
            }
            else if (!_isPositive)
            {
                _bars[index].transform.GetChild(add.quantity()).gameObject.SetActive(false);
                _moneyCounterInt += _cost[index];
                add.Change(-1);
                _costCounterInt += -_cost[index];
                _costCounter.text = "+" + Math.Abs(_costCounterInt);
            }
            if (_costCounterInt < 0)
                _costCounter.color = Color.green;
            else if (_costCounterInt > 0)
                _costCounter.color = Color.red;
            else if (_costCounterInt == 0)
                _costCounter.color = Color.white;
        }
    }

    public void IsPositive(bool isPositive)
    {
        _isPositive = isPositive;
    }

    private void BarSetup()
    {
        for (int i = 0; i < _bars.Length - 1; i++)
        {
            _foods[i].TryGetComponent(out IAdd add);
            int quantity = add.quantity();
            for (int x = 0; x < quantity; x++)
            {
                _bars[i].transform.GetChild(x + 1).gameObject.SetActive(true);
            }
        }
    }
    public bool CanAddFood(IAdd add)
    {
        return add.CanAddFood();
    }

    public void ExitButton()
    {
        if (_costCounterInt > 0)
            _endMoney.text = "Are you sure you want to spend " + Math.Abs(_costCounterInt) +
                             " For Buying this groceries?";
        else if (_costCounterInt < 0)
            _endMoney.text = "Are you sure you want to get " + Math.Abs(_costCounterInt) +
                             " For Selling this groceries?";
        else if (_costCounterInt == 0)
            _endMoney.text = "Are you sure you want to start the day without spending or getting any money?";
    }

    public void ConfirmButton()
    {
        EventManager.MoneyChanger?.Invoke(-_costCounterInt);
        EventManager.StartNextDay?.Invoke();
    }
}
