using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5.0f;
    [SerializeField] private GameObject _explosion = default;
    private SpawnManager _spawnManager = default;

    private void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
    }

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, 0.01f * _rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Laser laser))
        {
            if(_explosion) Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject);
        }
    }
}
