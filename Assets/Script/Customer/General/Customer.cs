using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour, IInteract
{
    
    private Transform _popUpPos;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }


    public bool Interact(bool isToAdd)
    {
        
    }

    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
}

