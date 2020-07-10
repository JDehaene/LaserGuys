using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _id = 1; 
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
        Vector2 move = _input.LThumb(_id) * 10 * Time.deltaTime;
        gameObject.transform.Translate(new Vector3(move.x, 0, move.y), Space.World);
    }
}
