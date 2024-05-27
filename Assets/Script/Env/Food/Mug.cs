using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour, IMug
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
}
