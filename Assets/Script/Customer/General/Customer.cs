using System.Collections;
using System.Collections.Generic;
using DS.ScriptableObjects;
using UnityEngine;

public class Customer : MonoBehaviour, IInteract, ICustomer
{
    
    private Transform _popUpPos;
    [SerializeField] private DSDialogueContainerSO _dialogueContainer;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }


    public bool Interact(bool isToAdd)
    {
        EventManager.OnStartingDialogue?.Invoke(_dialogueContainer, gameObject.name);
        return true;
    }

    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
}

