using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "ScriptableObj/Audio")]
public class AudioData : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip WalkSound; // Footsteps
    
    [Header("Environment Sounds")]
    public AudioClip DoorOpen; // Door Open Sound

    [Header("Event Sounds")] 
    public AudioClip CookingSound; // Cooking Sound
    
    




}
