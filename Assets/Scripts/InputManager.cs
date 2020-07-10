using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 LThumb(int playerID)
    {
        return new Vector2(Input.GetAxis("HorizontalL_P" + playerID), Input.GetAxis("VerticalL_P" + playerID));
    }

    public Vector2 RThumb(int playerID)
    {
        return new Vector2(Input.GetAxis("HorizontalR_P" + playerID), Input.GetAxis("VerticalR_P" + playerID));
    }
}
