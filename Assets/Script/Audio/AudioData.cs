using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "ScriptableObj/Audio")]
public class AudioData : ScriptableObject
{
    [Header("Player Sounds")]
    [SerializeField] public AudioClip sfx_walkingSound;
    [SerializeField] public AudioClip sfx_jumpSound;
    [SerializeField] public AudioClip sfx_landingSound;
    [SerializeField] public AudioClip sfx_dashSound;
    [SerializeField] public AudioClip sfx_groundPoundSound;
    [Header("Environment Sounds")]
    [SerializeField] public AudioClip sfx_lavaSound;
    [SerializeField] public AudioClip sfx_voidSound;
    [SerializeField] public AudioClip sfx_spikeSound;
    [SerializeField] public AudioClip sfx_dartsSound;
    [SerializeField] public AudioClip sfx_fireBreathSound;
    [SerializeField] public AudioClip sfx_openingDoorSound;
    [SerializeField] public AudioClip sfx_openingLockSound;
    [SerializeField] public AudioClip sfx_pushingCrateSound;
    [SerializeField] public AudioClip sfx_leverSound;
    [SerializeField] public AudioClip sfx_buttonSound;
    [SerializeField] public AudioClip sfx_floorSwitchSound;
    [SerializeField] public AudioClip sfx_timerSound;
    [SerializeField] public AudioClip sfx_pickupSound;
    [SerializeField] public AudioClip sfx_wallTrapSound;
    [Header("Event Sounds")]
    [SerializeField] public AudioClip sfx_deathSound;
    [SerializeField] public AudioClip sfx_victorySound;
    [SerializeField] public AudioClip sfx_looseSound;
    [Header("Event Sounds")]
    [SerializeField] public AudioClip sfx_buttoSound;




}
