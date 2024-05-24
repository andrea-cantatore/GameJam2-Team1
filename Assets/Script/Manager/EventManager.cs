using System;
using DS.ScriptableObjects;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<int, String, Material> OnGrillPickUp;
    public static Action<bool> OnCuttingInteraction;
    public static Action<bool> OnBookInteraction;
    public static Action<bool> OnBeerInteraction;
    public static Action<String, Material> OnCutted;
    public static Action<int> OnFullBeer;
    
    public static Action OnDialogueEnd;
    public static Action<DSDialogueContainerSO, String> OnStartingDialogue;

}

