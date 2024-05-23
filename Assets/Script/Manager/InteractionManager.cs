using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    
    public GameObject InteractionPannel;
    public GameObject InteractionText;
    
    void Awake()
    {
    
        #region Singleton
    
        if (Instance != null)
        {
            Destroy(transform.gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(transform.gameObject);
    
        #endregion
    }
}
