using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class LaserBehaviour : MonoBehaviour
{
    [SerializeField] private Material _cutMaterial = null;
    [SerializeField] private LayerMask _layerMask = 0;
    [SerializeField] private float _cutExplosion = 0;
    [SerializeField] private Transform[] _points = null;

    private float _maxLaserDistance = 0;
    private Transform _midPoint;
    private RaycastHit _hit;
    private bool _sliceCoolDown = false;

    private void Awake()
    {
        _midPoint = this.transform;
    }

    private void Update()
    {
        CheckSlice();

        _maxLaserDistance = Vector3.Distance(_points[0].position, _points[1].position);
        _midPoint.position = (_points[1].position+ _points[0].position)/2;
        _midPoint.LookAt(_points[1]);
    }

    #region Slicing
    public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(_hit.point, new Vector3(_maxLaserDistance, 0.01f, 0.1f), _midPoint.rotation, _layerMask);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, _cutMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, _cutMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, _cutMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
    }
    private void AddHullComponents(GameObject go)
    {
        go.layer = LayerMask.NameToLayer("Cuttable");
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(_cutExplosion, go.transform.position, 20);
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(_midPoint.position, _midPoint.transform.up, crossSectionMaterial);
    }

    private void CheckSlice()
    {
        Vector3 RayDir = _points[1].position - _points[0].position;

        Debug.DrawLine(_points[0].position, _points[1].position,Color.red);
        Debug.DrawRay(_points[0].position, RayDir, Color.blue);

        if (Physics.Raycast(_points[0].position, RayDir, out _hit, _maxLaserDistance,_layerMask))
        {
            Debug.Log("Intersect");
            if (!_sliceCoolDown)
            {
                Slice();
            }
            _sliceCoolDown = true;
        }
        else
            _sliceCoolDown = false;
    }
    #endregion

}
