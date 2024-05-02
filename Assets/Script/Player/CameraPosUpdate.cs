using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosUpdate : MonoBehaviour
{
    [SerializeField] private Transform _cameraPos;
    
    void Update()
    {
        transform.position = _cameraPos.position;   
    }
}
