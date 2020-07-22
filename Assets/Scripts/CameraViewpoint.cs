using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script that keeps a viewpoint radius, that if the player / camera
// enters it, the camera will take it in as a target to keep in view...
public class CameraViewpoint : MonoBehaviour
{
    [SerializeField] private Mesh _debugMesh;
    [SerializeField] private CameraBehaviour _camera;

    [Header("AlignAxes")]
    [SerializeField] private bool _alignX = true;
    [SerializeField] private bool _alignY = true;
    [SerializeField] private bool _alignZ = true;

    private Collider _collider;
    public Collider Collider { get => _collider;}

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true; // Set trigger, just to be sure :p
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_camera == null) return;
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Ugly :s, I'm tired...
            Vector3 currentEulers = _camera.transform.rotation.eulerAngles;
            Vector3 myEulers = transform.rotation.eulerAngles;
            Vector3 finalEulers;

            if (_alignX) finalEulers.x = myEulers.x; else finalEulers.x = currentEulers.x;
            if (_alignY) finalEulers.y = myEulers.y; else finalEulers.y = currentEulers.y;
            if (_alignZ) finalEulers.z = myEulers.z; else finalEulers.z = currentEulers.z;

            // Let camera allign this point's rotation
            _camera.AlignToRotation(finalEulers);
        }
    }


    private void OnDrawGizmos()
    {
        DrawArrow.ForGizmo(transform.position, transform.forward * 5, Color.green);
    }
}
