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
        EventManager.StartNextDay += PreDealerScreenDeactivation;
    }
    
    private void OnDisable()
    {
        EventManager.StartNextNight -= PreDealerScreen;
        EventManager.MoneyChanger -= MoneyChanger;
        EventManager.StartNextDay -= PreDealerScreenDeactivation;
    }
    
    private void PreDealerScreen()
    {
        EventManager.OnUiOpening?.Invoke(true);
        GameManager.Instance.UnlockCursor(null, null, null);
        _preDealerScreen.SetActive(true);
    }

    private void PreDealerScreenDeactivation()
    {
        EventManager.OnUiOpening?.Invoke(false);
        GameManager.Instance.LockCursor();
        _preDealerScreen.SetActive(false);
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
