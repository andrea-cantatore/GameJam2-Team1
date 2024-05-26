using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour, ICoin, IInteract
{
    private int _coins;
    private Transform _popUpPos;

    private void Awake()
    {
        _popUpPos = transform.GetChild(0);
    }

    public int ReturnCoins()
    {
        return _coins;
    }
    public void AddCoins(int coins)
    {
        _coins = coins;
    }

    public bool Interact(bool isToAdd)
    {
        return isToAdd;
    }
    public void InteractionPopUp()
    {
        InteractionManager.Instance.InteractionPannel.transform.position = _popUpPos.position;
        InteractionManager.Instance.InteractionText.GetComponent<TMPro.TextMeshProUGUI>().text =
            "press E to Interact " + gameObject.name;
    }
}
