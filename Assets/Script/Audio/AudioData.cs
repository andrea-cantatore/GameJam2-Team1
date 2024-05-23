using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "ScriptableObj/Audio")]
public class AudioData : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip WalkSound; // Footsteps

    [Header("Environment Sounds")]
    public AudioClip DoorOpen; // Door Open Sound (Morning)
    public AudioClip DoorClose; // Door Close Sound (Evening)
    public AudioClip Rooster; // Announces Day Starting
    public AudioClip Voices; // Customer Voices (3+ customers at the Conter)
    public AudioClip CustomerTalk; // Customer Talk Sound (Requires slower customer text generation)
    public AudioClip CoinsCollection; // Collect Coins From Conter

    [Header("Event Sounds")] 
    public AudioClip FoodOnGrill; // Food On Grill Sound
    public AudioClip FoodOnWood; // Food On Cutter, On EmptySpaces and on Dish
    public AudioClip Cutting; // Use Knife on Cutter (animation)
    public AudioClip DropCauldron; // Food Dropped in Cauldron
    public AudioClip PickUpCauldron; // Food PickedUp from Cauldron
    public AudioClip PageChange; // BookPage Changes
    public AudioClip BeerPouring; // Pouring Beer In Glass
    public AudioClip Glass; // PickUp Mugs, Place Mugs

    [Header("UI Sounds")]
    public AudioClip BellRing; // Customer announcement Sound, UI Bell Element animated
    public AudioClip ButtonPressed; // UI Button Pressed (Generic)




}
