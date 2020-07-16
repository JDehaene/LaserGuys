using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private int _id = 1;
    public int ID { get { return _id; } }

    [Header("Movement")]
    [SerializeField] private float _accelerationPower = 10.0f;
    [SerializeField] private float _maxSpeed = 20.0f;
    [SerializeField] private float _drag = 2.0f;
    [SerializeField] private float _jumpForce = 10;
    [SerializeField] private float _weight = -40.0f;
    [SerializeField] private int _amountOfJumps = 2;

    private int _jumpsLeft = 0;
    private Vector3 _velocity = new Vector3();
    private Vector3 _acceleration = new Vector3();

    private InputManager _input;
    private CharacterController _controller;

    private void Awake()
    {
        _input = FindObjectOfType<InputManager>();
        if (!_input) { };

        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        ResetJumps();
    }

    void Update()
    {
        ProcessInput();

        _velocity += _acceleration * Time.deltaTime;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        Vector3 localVel = transform.TransformVector(_velocity);
        _controller.Move(localVel * Time.deltaTime);

        if (_velocity == Vector3.zero) return;

        float newScale = _velocity.magnitude - (_drag * Time.deltaTime);
        _velocity = _velocity.normalized * newScale;

        // Apply gravity:
        _acceleration.y = _weight;
    }

    void ProcessInput()
    {
        Vector2 input = _input.LThumb(_id);
        input = input.normalized * _accelerationPower;

        _acceleration.x = input.x;
        _acceleration.z = input.y;

        if (_input.ADown(_id)) DoJump();
    }

    #region Jumping
    void DoJump()
    {
        if (_jumpsLeft <= 0) return;

        _acceleration.y = _jumpForce * 1000;
        _velocity.y = 0;

        --_jumpsLeft;
    }
    void ResetJumps()
    {
        _jumpsLeft = _amountOfJumps;
        _velocity.y = 0;
    }
    #endregion


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("JumpReset")) ResetJumps();
    }
}
