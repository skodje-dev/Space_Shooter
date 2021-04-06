using System;
using UnityEngine;
public class Detector : MonoBehaviour
{
    private GameObject _colliderGO;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other)
        {
            _colliderGO = other.gameObject;
        }
    }

    public bool CheckType(string checkIfType)
    {
        return _colliderGO ? _colliderGO.GetComponent(Type.GetType(checkIfType)) : false;
    }

    public bool CheckTag(string checkIfTag)
    {
        return _colliderGO && _colliderGO.CompareTag(checkIfTag);
    }
}
