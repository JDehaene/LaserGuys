﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private float _smoothTime = 0.5f;
    [SerializeField] private float _rotationSpeed = 0.5f;
    public List<Transform> targets;
    private Vector3 _velocity;
    private Quaternion _targetRotation;

    private void Awake()
    {
        _targetRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) return;

        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 mid = GetMidPoint();
        transform.localPosition = Vector3.SmoothDamp(transform.position, mid, ref _velocity, _smoothTime);
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetMidPoint()
    {
        if (targets.Count == 1) return targets[0].position;

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; ++i)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    public void AlignToRotation(Quaternion rotation)
    {
        _targetRotation = rotation;
    }

    public void AlignToRotation(Vector3 eulers)
    {
        AlignToRotation(Quaternion.Euler(eulers));
    }
}
