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
            _doorAnimator.SetBool("IsOpen", true);
            wait();
        }
        
    }
    
    private void OnCollisionExit(Collision other)
    {
        if(other.transform.TryGetComponent(out ICustomer customer))
        {
            _doorAnimator.SetBool("IsOpen", false);
            wait();
        }
    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX(_audioData.DoorClose, transform);
    }
}
