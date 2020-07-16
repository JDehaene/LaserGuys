using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private Transform _object;
    [SerializeField] private float _speed;

    private int _targetIdx = 0;
    private float _distanceBias = 0.05f;

    private void Awake()
    {
        if (_points.Length == 0) return;

        _object.position = _points[_targetIdx].position;
    }

    private void Update()
    {
        if (_points.Length == 0) return;

        _object.position = Vector3.MoveTowards(_object.position, _points[_targetIdx].position, Time.deltaTime * _speed);
        if (Vector3.Distance(_object.position, _points[_targetIdx].position) <= _distanceBias) SetNextIdx();
    }

    private void SetNextIdx()
    {
        ++_targetIdx;
        if (_targetIdx >= _points.Length) _targetIdx = 0;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _points.Length; ++i)
        {
            if (i + 1 >= _points.Length) continue;
            Gizmos.DrawLine(_points[i].position, _points[i + 1].position);
        }
    }
}
