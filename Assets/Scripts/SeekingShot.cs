using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingShot : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private float _selfDestructTimer = 4.0f;
    private Transform _target;

    void Start()
    {
        AcquireTarget();
        Destroy(gameObject, _selfDestructTimer);
    }

    void Update()
    {
        if (_target.gameObject.activeInHierarchy)
        {
            transform.up = _target.position.normalized;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
        }
        else
        {
            AcquireTarget();
        }
    }

    private void AcquireTarget()
    {
        var enemies = FindObjectsOfType<Enemy>();
        int randomTarget = Random.Range(0, enemies.Length);
        _target = enemies[randomTarget].transform;
    }
}
