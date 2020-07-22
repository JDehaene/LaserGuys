using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnCutAction(GameObject cutObject);
    public static event OnCutAction OnCut;

    static public void NotifyOnCut(GameObject cutObj)
    {
        OnCut(cutObj);
    }
}
