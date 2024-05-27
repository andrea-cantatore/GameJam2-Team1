using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour, IBowl
{

    [SerializeField] private GameObject[] _childs;
    private int[] _childsID = {1,2,4,8,16,32,64,128,256,512};
    private int _generalID;
    
    public int IDReturner()
    {
        for(int i = 0; i < _childs.Length; i++)
        {
            if(_childs[i].activeSelf)
            {
                _generalID += _childsID[i];
            }
        }
        int temp = _generalID;
        _generalID = 0;
        return temp;
    }
    
    public bool[] ReleaseDish()
    {
        bool[] activeFoods = new bool[_childs.Length];
        for (int i = 0; i < _childs.Length; i++)
        {
            activeFoods[i] = _childs[i].activeSelf;
        }
        return activeFoods;
    }
}
