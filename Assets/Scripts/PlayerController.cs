using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] private int _id = 1;
    public int ID { get { return _id; } }

    [Header("Movement")]
    [SerializeField] private float _accelerationPower = 10.0f;
    [SerializeField] private float _maxSpeed = 20.0f;
    [SerializeField] private float _maxJumpSpeed = 20.0f;
    [SerializeField] private float _horizontalDrag = 2.0f;
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

        ResetJumps();
    }

    void Update()
    {
        ProcessInput();
        DoHorizontalMovement();
        DoVerticalMovement();
        DoRotation();
        DoHorizontalDrag();

        // Apply gravity:
        _acceleration.y = _weight;
    }

    void ProcessInput()
    {
        Vector2 input = _input.LThumb(_id);
        if (input != Vector2.zero) input = input.normalized * _accelerationPower;

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

    #region Movement
    void DoHorizontalMovement()
    {
        Vector2 horVel = new Vector2(_velocity.x, _velocity.z);
        Vector2 horAcc = new Vector2(_acceleration.x, _acceleration.z);
        horVel += horAcc * Time.deltaTime;
        horVel = Vector2.ClampMagnitude(horVel, _maxSpeed);

        Vector3 camV = Camera.main.transform.TransformVector(horVel);
        Vector3 localVel = new Vector3(camV.x, 0, camV.z);

        Debug.DrawLine(transform.position, transform.position + localVel);
        _controller.Move(localVel * Time.deltaTime);

        _velocity.x = horVel.x;
        _velocity.z = horVel.y;
    }

    void DoVerticalMovement()
    {
        float vertVel = _velocity.y;
        float vertAcc = _acceleration.y;
        vertVel += vertAcc * Time.deltaTime;
        vertVel = Mathf.Clamp(vertVel, -100000, _maxJumpSpeed);

        _controller.Move(new Vector3(0, vertVel, 0) * Time.deltaTime);

        if (_controller.isGrounded) vertVel = 0;
        _velocity.y = vertVel;
    }
    #endregion

    void DoRotation()
    {
        // if no player input, no rotation
        if (_input.LThumb(_id) == Vector2.zero) return;

        // Set rotation
        Vector3 localVel = Camera.main.transform.TransformVector(_velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(localVel.x, 0, localVel.z)), 1);
    }

    void DoHorizontalDrag()
    {
        Vector2 horVel = new Vector2(_velocity.x, _velocity.z);
        if (horVel == Vector2.zero) return;

        float newScale = horVel.magnitude - (_horizontalDrag * Time.deltaTime);

        if (newScale <= 0.001f) horVel = Vector2.zero;
        else horVel = horVel.normalized * newScale;

        _velocity.x = horVel.x;
        _velocity.z = horVel.y;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("JumpReset")) ResetJumps();
    }
}
