using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _cutPlane = null;
    [SerializeField] private Material _cutMaterial = null;
    [SerializeField] private LayerMask _layerMask = 0;
    [SerializeField] private int _cuttingBoxSize = 0, _laserWidth = 0;
    [SerializeField] private float _cutExplosion = 0;
    [SerializeField] private Transform _midPoint;

    [SerializeField] private Transform[] _playerLocations = null;

    private RaycastHit _hit;
    private bool _sliceCoolDown = false;

    private void Update()
    {
        CheckSlice();
        _midPoint.position = (_playerLocations[1].position+ _playerLocations[0].position)/2;
        _midPoint.LookAt(_playerLocations[1]);
    }

    public void Slice()
    {

        Collider[] hits = Physics.OverlapBox(_hit.point, new Vector3(_cuttingBoxSize, 0.01f, _laserWidth), _midPoint.rotation, _layerMask);

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

        return obj.Slice(_cutPlane.position, _cutPlane.up, crossSectionMaterial);
    }

    private void CheckSlice()
    {
        Debug.DrawLine(_playerLocations[0].position, _playerLocations[1].position,Color.red);

        if (Physics.Raycast(_playerLocations[0].position, _playerLocations[1].position,out _hit, _layerMask))
        {
            Debug.Log("Intersectg");
            if (!_sliceCoolDown)
            {
                Slice();
            }
            _sliceCoolDown = true;
        }
        else
            _sliceCoolDown = false;
    }


}
