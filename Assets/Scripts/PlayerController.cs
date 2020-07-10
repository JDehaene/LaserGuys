using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager _input;

    private void Awake()
    {
        _input = FindObjectOfType<InputManager>();
        if (!_input) { };
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        Vector2 move = _input.LThumb(1) * 10;
        Debug.Log("Input: " + move);
        gameObject.transform.Translate(new Vector3(move.x, 0, move.y), Space.World);
    }
}
