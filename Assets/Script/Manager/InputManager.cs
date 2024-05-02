 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    public static ActionMap actionMap;

    
    static InputManager()
    {
        actionMap = new ActionMap();
        actionMap.Enable();
        actionMap.Menu.Disable();
    }

    public static void SwitchToMenuInput()
    {
        Time.timeScale = 0;
        actionMap.PlayerInput.Disable();
        actionMap.Menu.Enable();
        
    }

    public static void SwitchToPlayerInput()
    {
        Time.timeScale = 1;
        actionMap.PlayerInput.Enable();
        actionMap.Menu.Disable();
    }

   

    
    
}
