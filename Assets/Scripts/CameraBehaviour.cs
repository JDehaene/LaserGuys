using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime = 0.5f;
    public List<Transform> targets;
    private Vector3 _velocity;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }
    private void LateUpdate()
    {
        if (targets.Count == 0) return;

        Move();
    }

    private void Move()
    {
        Vector3 mid = GetMidPoint();

        transform.position = Vector3.SmoothDamp(transform.position, mid + _offset, ref _velocity, _smoothTime);
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
}
