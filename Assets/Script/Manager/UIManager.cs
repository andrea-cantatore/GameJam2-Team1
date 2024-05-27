using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _preDealerScreen;
    [SerializeField] private TMP_Text _moneyCounter;
    public int MoneyCounterInt;
    [SerializeField] private int _startMoney = 100;
    public static UIManager Instance;
    [SerializeField] private GameObject _winScreen, _loseScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _moneyCounter.text = MoneyCounterInt.ToString();
    }
    
    private void OnEnable()
    {
        EventManager.StartNextNight += PreDealerScreen;
        EventManager.MoneyChanger += MoneyChanger;
        EventManager.StartNextDay += PreDealerScreenDeactivation;
        EventManager.OnWin += WinScreen;
        EventManager.OnLose += LoseScreen;
    }
    
    private void OnDisable()
    {
        EventManager.StartNextNight -= PreDealerScreen;
        EventManager.MoneyChanger -= MoneyChanger;
        EventManager.StartNextDay -= PreDealerScreenDeactivation;
        EventManager.OnWin -= WinScreen;
        EventManager.OnLose -= LoseScreen;
    }
    
    private void PreDealerScreen()
    {
        EventManager.OnUiOpening?.Invoke(true);
        _preDealerScreen.SetActive(true);
    }

    private void PreDealerScreenDeactivation()
    {
        EventManager.OnUiOpening?.Invoke(false);
        _preDealerScreen.SetActive(false);
    }
    
    public void WinScreen()
    {
        _winScreen.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void LoseScreen()
    {
        _loseScreen.SetActive(true);
        Time.timeScale = 0;
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
    
    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
