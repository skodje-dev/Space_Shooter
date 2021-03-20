using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField, Range(1, 7)]private int _lives = 3;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private GameObject _laserPrefab = default;
    [SerializeField] private float _shootDelay = 0.4f;

    private float _canFire = 0f;
    private float _xMaxBounds = 10f;
    private float _xMinBounds = -10f;
    private float _yMaxBounds = .5f;
    private float _yMinBounds = -3f;

    private void Start()
    {
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        var movement = GetInput();
        if (Input.GetKeyDown(KeyCode.Space) && _canFire < Time.time)
        {
            FireLaser();
        }

        var velocity = movement * Time.deltaTime * _speed;
        transform.Translate(velocity);
        CheckPosition();
    }

    private void FireLaser()
    {
        _canFire = Time.time + _shootDelay;
        Instantiate(_laserPrefab, transform.position, Quaternion.identity);
    }

    private static Vector3 GetInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        return new Vector3(hInput, vInput);
    }

    private void CheckPosition()
    {
        var position = transform.position;
        position = new Vector3(position.x, Mathf.Clamp(position.y, _yMinBounds, _yMaxBounds));
        
        if (transform.position.x < _xMinBounds)
            position = new Vector3(_xMaxBounds, position.y);
        if (position.x > _xMaxBounds)
            position = new Vector3(_xMinBounds, position.y);

        transform.position = position;
    }

    public void Damage()
    {
        _lives--;
        if (_lives < 1)
        {
            SpawnManager.GameOver();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            Destroy(other.gameObject);
            Damage();
        }
    }
}
