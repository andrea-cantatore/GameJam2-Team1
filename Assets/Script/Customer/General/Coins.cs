using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour, ICoin
{
    private int _coins;
    
    public int ReturnCoins()
    {
        return _coins;
    }
    public void AddCoins(int coins)
    {
        _coins = coins;
    }

}
