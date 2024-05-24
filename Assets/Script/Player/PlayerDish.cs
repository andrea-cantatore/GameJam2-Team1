using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDish : MonoBehaviour, IDish
{
    [SerializeField] private GameObject[] _foods;
    [SerializeField] private bool[] activeFoods;
    
    private bool _isMeetOn;
    private bool _isNotPotatoOn;
    private bool _isFishOn;
    private string[] _foodTags = {"SteakPick", "PotatoPick", "VegetablePick", "SteakSliced", "ChickenPick",
        "ChickenSliced", "FishPick", "TomatoPick", "FishPick", "FishSliced"};

    private void Awake()
    {
        activeFoods = new bool[_foods.Length];
    }
    public bool GetDish(GameObject food, bool isSliced)
    {
        if ((food.tag == _foodTags[1] || food.tag == _foodTags[2] || food.tag == _foodTags[7]) && !isSliced)
        {
            return false;
        }
        if(_isFishOn && (food.tag == _foodTags[2] || food.tag == _foodTags[7]))
        {
            return false;
        }
        if (food.tag == _foodTags[1] || food.tag == _foodTags[2] || food.tag == _foodTags[7])
        {
            foreach (GameObject obj in _foods)
            {
                if(obj.tag == food.tag)
                {
                    obj.SetActive(true);
                    if(obj.tag == _foodTags[2] || obj.tag == _foodTags[7])
                    {
                        _isNotPotatoOn = true;
                    }
                    
                    return true;
                }
            }
        }
        if (_isMeetOn)
        {
            return false;
        }
        if(food.tag == _foodTags[8] && _isNotPotatoOn)
        {
            return false;
        }
        if(food.tag == _foodTags[8])
        {
            _isFishOn = true;
        }

        if (!isSliced)
        {
            foreach (GameObject obj in _foods)
            {
                if(obj.tag == food.tag)
                {
                    obj.SetActive(true);
                    _isMeetOn = true;
                    return true;
                }
            }
        }
        if(food.tag == _foodTags[0])
        {
            food.tag = _foodTags[3];
        }
        if(food.tag == _foodTags[4])
        {
            food.tag = _foodTags[5];
        }
        if(food.tag == _foodTags[8])
        {
            food.tag = _foodTags[9];
        }
        foreach (GameObject obj in _foods)
        {
            if(obj.tag == food.tag)
            {
                obj.SetActive(true);
                _isMeetOn = true;
                return true;
            }
        }
        
        return false;
    }
    
    public bool[] ReleaseDish()
    {
        for (int i = 0; i < _foods.Length; i++)
        {
            activeFoods[i] = _foods[i].activeSelf;
        }
        return activeFoods;
    }

    public bool[] ActiveFood()
    {
        int childCount = transform.childCount;
        bool[] activeChildren = new bool[childCount];

        for (int i = 0; i < childCount; i++)
        {
            activeChildren[i] = transform.GetChild(i).gameObject.activeSelf;
        }

        return activeChildren;
    }
}
