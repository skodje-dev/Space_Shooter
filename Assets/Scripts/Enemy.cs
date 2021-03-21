using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab = default;

    [Header("Fire Settings")]
    private float _firstShotDelay = 1.0f;
    [SerializeField, Range(0.5f, 5.0f)] private float _shootDelay = 2.0f;
    [SerializeField, Range(0, 100)] private int _variancePct = 0;
    private float _variance;

    private float _canFire;
    void Start()
    {
        _canFire = Time.time + _firstShotDelay;
        _variance = _shootDelay * _variancePct / 100;
    }

    void Update()
    {
        CalculateMovement();
        if (_canFire < Time.time)
        {
            GameObject laserGO = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] enemyLaser = laserGO.GetComponentsInChildren<Laser>();
            foreach (var laser in enemyLaser)
            {
                laser.tag = "EnemyLaser";
                laser.SetEnemyLaser();
            }
            
            if(_variancePct == 0)
                _canFire = Time.time + _shootDelay;
            else
                _canFire = Time.time + Random.Range(_shootDelay - _variance, _shootDelay + _variance);
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -8f)
        {
            float randomXPos = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomXPos, 8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.TryGetComponent(out Player player))
        {
            player.Damage();
            Destroy(gameObject);
        }
    }
}
