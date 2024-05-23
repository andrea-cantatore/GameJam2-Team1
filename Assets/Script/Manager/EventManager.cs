using System;
using DS.ScriptableObjects;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<int, String, Material> OnGrillPickUp;
    public static Action<bool> OnCuttingInteraction;
    public static Action<String, Material> OnCutted;
    
    public static Action OnDialogueEnd;
    public static Action<DSDialogueContainerSO, String> OnStartingDialogue;

}

