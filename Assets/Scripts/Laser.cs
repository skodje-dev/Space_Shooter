using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField, Tooltip("Laser will travel upwards based on rotation")] private float _speed = 7.0f;
    private bool _enemyLaser = false;
    private Vector3 _direction;

    private void Start()
    {
        _direction = _enemyLaser ? Vector3.down : Vector3.up;
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.position += _direction * Time.deltaTime * _speed;
        if (Mathf.Abs(transform.position.y) > 8.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetEnemyLaser()
    {
        _enemyLaser = true;
    }
}
