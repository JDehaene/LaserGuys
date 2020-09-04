using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _triggerCutObj;

    [SerializeField] private Transform _posIdle;
    [SerializeField] private Transform _posActivated;

    private bool _isActivated = false;

    private void Awake()
    {
        EventManager.OnCut += OnCut;
    }

    private void Update()
    {   
        if (_isActivated) transform.position = _posActivated.position;
        else transform.position = _posIdle.position;
    }

    private void OnCut(GameObject cutObj)
    {
        if (cutObj == _triggerCutObj)
        {
            Debug.Log("Lift opening!");
            _isActivated = true;
        }
    }
}
