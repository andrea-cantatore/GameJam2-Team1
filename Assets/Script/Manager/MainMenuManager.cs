using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _creditsMenu, _settingsMenu, _mainMenu, _controlMenu, _controller, _keyboard, _mouse;


    public void Credits(bool isToShow)
    {
        if (isToShow)
        {
            _mainMenu.SetActive(false);
        }
        else
        {
            _mainMenu.SetActive(true);
        }
        
        _creditsMenu.SetActive(isToShow);
    }

    public void Settings(bool isToShow)
    {
        if (isToShow)
        {
            _mainMenu.SetActive(false);
        }
        else
        {
            _mainMenu.SetActive(true);
        }
        
        _settingsMenu.SetActive(isToShow);
    }
    public void Controlls(bool isToShow)
    {
        if (isToShow)
        {
            _mainMenu.SetActive(false);
        }
        else
        {
            _mainMenu.SetActive(true);
        }
        
        _controlMenu.SetActive(isToShow);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ControllSwapper()
    {
        if (_keyboard.activeSelf)
        {
            _keyboard.SetActive(false);
            _mouse.SetActive(false);
            _controller.SetActive(true);
        }
        else
        {
            _keyboard.SetActive(true);
            _mouse.SetActive(true);
            _controller.SetActive(false);
        }
    }
    
    
    
    
    
}
