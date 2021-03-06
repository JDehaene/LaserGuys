﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

[RequireComponent(typeof(LineRenderer))]
public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerController[] _players = null;

    [Header("Slicing")]
    [SerializeField] private Material _cutMaterial = null;
    [SerializeField] private LayerMask _layerMask = 0;
    [SerializeField] private float _cutExplosion = 0;
    [SerializeField] private PhysicMaterial _pillarMaterial = null;
    public GameObject Sparks = null;

    [Header("Spring")]
    [SerializeField] private Vector2 _minMaxDistance = new Vector2();
    [SerializeField] private float _springStrength = 10f;


    private LineRenderer _lineRenderer = null;
    private float _maxLaserDistance = 0;
    private Transform _midPoint = null;
    private RaycastHit _hit;
    private GameObject _sliceTarget, _previousSliceTarget = null;
    private bool _sliceCoolDown = false;

    private void Awake()
    {
        _midPoint = this.transform;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;

        // Setup spring connection:
        //SpringJoint spring = _players[0].gameObject.AddComponent<SpringJoint>();
       // spring.connectedBody = _players[1].GetComponent<Rigidbody>();
        //spring.minDistance = _minMaxDistance.x;
        //spring.maxDistance = _minMaxDistance.y;
        //spring.spring = _springStrength;
    }


    private void Update()
    {
        CheckSlice();

        _maxLaserDistance = Vector3.Distance(_players[0].transform.position, _players[1].transform.position);
        _midPoint.position = (_players[1].transform.position+ _players[0].transform.position)/2;
        _midPoint.LookAt(_players[1].transform.position);

        for (int i = 0; i < 2; ++i) _lineRenderer.SetPosition(i, _players[i].transform.position);
    }

    #region Cutting
    public void Slice()
    {
        SlicedHull hull = SliceObject(_hit.transform.gameObject, _cutMaterial);
        if (hull != null)
        {
            GameObject bottom = hull.CreateLowerHull(_hit.transform.gameObject, _cutMaterial);
            GameObject top = hull.CreateUpperHull(_hit.transform.gameObject, _cutMaterial);
            AddHullComponents(bottom);
            AddHullComponents(top);
            Destroy(_hit.transform.gameObject);
        }
    }
    private void AddHullComponents(GameObject go)
    {
        go.AddComponent<NonSliceable>();

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.material = _pillarMaterial;
        collider.convex = true;

        rb.AddExplosionForce(_cutExplosion, go.transform.position, 20);
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        EventManager.NotifyOnCut(obj);
        return obj.Slice(_midPoint.position, _midPoint.transform.up, crossSectionMaterial);
    }

    private void CheckSlice()
    {
        Vector3 RayDir = _players[1].transform.position - _players[0].transform.position;
        Ray Ray = new Ray(_players[0].transform.position, RayDir);


        if (Physics.Raycast(Ray, out _hit, _maxLaserDistance, _layerMask))
        {
            Slice();
        }

    }
    #endregion
}
