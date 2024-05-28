using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator;
    
    [SerializeField] private AudioData _audioData;

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.TryGetComponent(out ICustomer customer))
        {
            AudioManager.instance.PlaySFX(_audioData.DoorOpen, transform);
            _doorAnimator.SetBool("IsOpen", true);
        }
        
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.transform.TryGetComponent(out ICustomer customer))
        {
            AudioManager.instance.PlaySFX(_audioData.DoorClose, transform);
            _doorAnimator.SetBool("IsOpen", false);
        }
    }
    
    
}
