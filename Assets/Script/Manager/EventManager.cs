using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<String> OnGrillInteraction;
    public static Action<int, String> OnGrillPickUp;
}

