using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName ="ScriptableObjects/EventSO")]
public class EventSO : ScriptableObject
{
    public Action CurrentAction;
}
