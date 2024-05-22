using System;
using DS.ScriptableObjects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        
    }
    
    private void OnEnable()
    {
        EventManager.OnStartingDialogue += UnlockCursor;
        EventManager.OnDialogueEnd += LockCursor;
    }
    private void OnDisable()
    {
        EventManager.OnStartingDialogue -= UnlockCursor;
        EventManager.OnDialogueEnd -= LockCursor;
    }
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void UnlockCursor(DSDialogueContainerSO dialogueContainer, String dialogue)
    
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}

