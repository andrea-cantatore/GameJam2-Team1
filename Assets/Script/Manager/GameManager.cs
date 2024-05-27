using System;
using DS.ScriptableObjects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        LockCursor();
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
    
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void UnlockCursor(DSDialogueContainerSO dialogueContainer, String dialogue, Customer customer)
    
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}

