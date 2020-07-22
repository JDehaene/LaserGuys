using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Observer class with function OnNotify()

public class OnCut : MonoBehaviour
{
    public delegate void DoOnCut();
    public DoOnCut _doOnCut;
}
