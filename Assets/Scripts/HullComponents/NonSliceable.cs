using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonSliceable : MonoBehaviour
{
    [SerializeField] private float _restTime = 3;
    [SerializeField] private GameObject _cooldownSparks;
    private void Awake()
    {
        this.gameObject.layer = LayerMask.NameToLayer("JumpReset");
        _cooldownSparks = FindObjectOfType<LaserBehaviour>().Sparks;
        StartCoroutine("ReturnSliceability");
    }
    IEnumerator ReturnSliceability()
    {
        yield return new WaitForSeconds(_restTime);

        GameObject Sparks = Instantiate(_cooldownSparks, this.transform.position, Quaternion.identity);
        Destroy(Sparks, 1);
        this.gameObject.layer = LayerMask.NameToLayer("Cuttable");
        Destroy(this.GetComponent<NonSliceable>());
    }
}
