using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BumperArm : MonoBehaviour
{
    [SerializeField] private Transform _item;
    [SerializeField] private float _armDistance = 10;

    void LateUpdate()
    {
        if (!_item) return;
        Debug.DrawLine(transform.position, _item.position);

        _item.transform.position = transform.position - transform.forward * _armDistance;
    }
}
