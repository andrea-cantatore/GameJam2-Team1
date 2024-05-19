using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<int, String, Material> OnGrillPickUp;
    public static Action<bool> OnCuttingInteraction;
    public static Action<String, Material> OnCutted;

}

