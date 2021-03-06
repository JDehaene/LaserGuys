﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public Vector2 LThumb(int playerID)
    {
        Vector2 val = new Vector2(Input.GetAxis("HorizontalL_P" + playerID), Input.GetAxis("VerticalL_P" + playerID));
        return val;
    }

    public Vector2 RThumb(int playerID)
    {
        Vector2 val = new Vector2(Input.GetAxis("HorizontalR_P" + playerID), Input.GetAxis("VerticalR_P" + playerID));
        return val;
    }

    public bool ADown(int playerID)
    {
        return Input.GetButtonDown("A_P" + playerID);
    }

    public bool AUp(int playerID)
    {
        return Input.GetButtonUp("A_P" + playerID);
    }
}
