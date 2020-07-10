using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class SliceBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _cutPlane = null;
    [SerializeField] private Material _cutMaterial = null;
    [SerializeField] private LayerMask _layerMask = 0;
    [SerializeField] private int _cuttingBoxSize = 0;
    private void Start()
    {
    }
    public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(_cutPlane.position, new Vector3(5, 0.1f, 5), _cutPlane.rotation, _layerMask);

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
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, go.transform.position, 20);
    }

    private SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(_cutPlane.position, _cutPlane.up, crossSectionMaterial);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Slicing active");
        Slice();
    }
}
