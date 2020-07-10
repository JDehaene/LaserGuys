using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 LThumb(int playerID)
    {
        Vector2 val = new Vector2(Input.GetAxis("HorizontalL_P" + playerID), Input.GetAxis("VerticalL_P" + playerID));
        Debug.Log(val);
        return val;
    }

    public Vector2 RThumb(int playerID)
    {
        Vector2 val = new Vector2(Input.GetAxis("HorizontalR_P" + playerID), Input.GetAxis("VerticalR_P" + playerID));
        Debug.Log(val);
        return val;
    }
}
