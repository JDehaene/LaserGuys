﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private int _id = 1;
    public int ID { get { return _id; } }

    [Header("Movement")] 
    [SerializeField] private float _moveSpeed = 10;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private int _amountOfJumps = 2;

    private int _jumpsLeft = 0;

    private InputManager _input;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _input = FindObjectOfType<InputManager>();
        if (!_input) { };

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ResetJumps();
    }

    void Update()
    {
        ProcessInput();
    }

    #region InputHandling
    void ProcessInput()
    {
        Vector2 input = _input.LThumb(_id);
        input = input.normalized * _moveSpeed;

        _rigidbody.velocity = new Vector3(input.x, _rigidbody.velocity.y, input.y);

        if (_input.ADown(_id)) DoJump();
    }
    #endregion

    #region Jumping
    void DoJump()
    {
        if (_jumpsLeft <= 0) return;
        _rigidbody.AddForce(new Vector3(0, 1, 0) * _jumpForce, ForceMode.VelocityChange);

        --_jumpsLeft;
    }
    void ResetJumps()
    {
        _jumpsLeft = _amountOfJumps;
    }
    #endregion


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(LayerMask.NameToLayer("JumpReset"));
        if (collision.gameObject.layer == LayerMask.NameToLayer("JumpReset")) ResetJumps();
    }
}
