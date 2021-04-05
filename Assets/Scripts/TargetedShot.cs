using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class TargetedShot : MonoBehaviour
{
    private bool _reachedTargetingPosition = false;
    private Vector3 _targetingPosition = new Vector3(0, 1, 0);
    private Transform _enemyTarget = null;
    [SerializeField] private float _speedToTargetingPos = 2.0f;
    [SerializeField] private float _targetAcquisitionTime = 0.8f;
    [SerializeField] private float _speed = 10.0f;


    void Update()
    {
        if (!_reachedTargetingPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetingPosition, Time.deltaTime * _speedToTargetingPos);
            _reachedTargetingPosition = Vector3.Distance(transform.position, _targetingPosition) < 0.1f;
        }
        else if (!_enemyTarget)
        {
            StartCoroutine(AcquireNearestTarget());
        }
        else
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _enemyTarget.position, Time.deltaTime * _speed);
        }
    }

    private IEnumerator AcquireNearestTarget()
    {
        yield return new WaitForSeconds(_targetAcquisitionTime);
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestEnemyDistance = Mathf.Infinity;
        if (enemies.Length == 0)
        {
            Destroy(gameObject);
        }
        
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestEnemyDistance)
            {
                closestEnemyDistance = distanceToEnemy;
                _enemyTarget = enemy.transform;
            }
        }
        
    }
}
