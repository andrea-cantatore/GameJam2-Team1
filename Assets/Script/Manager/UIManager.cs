using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _preDealerScreen;
    [SerializeField] private TMP_Text _moneyCounter;
    public int MoneyCounterInt;
    [SerializeField] private int _startMoney = 100;
    private void Start()
    {
        _moneyCounter.text = MoneyCounterInt.ToString();
    }
    private void OnEnable()
    {
        EventManager.StartNextNight += PreDealerScreen;
        EventManager.MoneyChanger += MoneyChanger;
    }
    
    private void OnDisable()
    {
        EventManager.StartNextNight -= PreDealerScreen;
        EventManager.MoneyChanger -= MoneyChanger;
    }
    
    public void PreDealerScreen()
    {
        _preDealerScreen.SetActive(true);
    }
    
    public void StartNextDay()
    {
        EventManager.StartNextDay?.Invoke();
    }
    
    public void MoneyChanger(int money)
    {
        MoneyCounterInt += money;
        _moneyCounter.text = MoneyCounterInt.ToString();
    }
}
