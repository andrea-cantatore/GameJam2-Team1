using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDish : MonoBehaviour
{
    private GameObject[] _foods;
    private bool _isMeetOn;
    private bool _isNotPotatoOn;
    private string[] _foodTags = {"SteakPick", "PotatoPick", "VegetablePick", "SteakSliced", "ChickenPick",
        "ChickenSliced", "FishPick", "TomatoPick", "FishPick", "FishSliced"};
    public bool GetDish(GameObject food, bool isSliced)
    {
        if ((food.tag == _foodTags[1] || food.tag == _foodTags[2] || food.tag == _foodTags[7]) && !isSliced)
        {
            Debug.Log("is not sliced");
            return false;
        }
        if (food.tag == _foodTags[1] || food.tag == _foodTags[2] || food.tag == _foodTags[7])
        {
            foreach (GameObject obj in _foods)
            {
                if(obj.tag == food.tag)
                {
                    obj.SetActive(true);
                    if(obj.tag == _foodTags[1] || obj.tag == _foodTags[7])
                    {
                        _isNotPotatoOn = true;
                    }
                    
                    return true;
                }
            }
        }
        if (_isMeetOn)
        {
            Debug.Log("is meet on");
            return false;
        }
        if(food.tag == _foodTags[8] && _isNotPotatoOn)
        {
            Debug.Log("is not potato on");
            return false;
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
        Debug.Log("something is wrong");
        return false;
    }
    
}
