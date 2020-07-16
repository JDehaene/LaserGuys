﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script that keeps a viewpoint radius, that if the player / camera
// enters it, the camera will take it in as a target to keep in view...
[RequireComponent(typeof(SphereCollider))]
public class CameraViewpoint : MonoBehaviour
{
    [SerializeField] private Mesh _debugMesh;

    private SphereCollider _collider;
    public Collider Collider { get => _collider;}


    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawMesh(_debugMesh, transform.position, transform.rotation, transform.localScale);
    }
}
