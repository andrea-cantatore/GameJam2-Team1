using System;
using DS.ScriptableObjects;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<int, String, Material> OnGrillPickUp;
    public static Action<bool> OnCuttingInteraction;
    public static Action<bool> OnBookInteraction;
    public static Action<bool> OnBeerInteraction;
    public static Action<String> OnCutted;
    public static Action<int> OnFullBeer;
    
    public static Action OnDialogueEnd;
    public static Action<DSDialogueContainerSO, String, Customer> OnStartingDialogue;
    public static Action<bool> IsNight;
    public static Action StartNextDay;
    public static Action StartNextNight;
    public static Action<Transform> OnCustomerLeave;
    public static Action<int> MoneyChanger;

}

