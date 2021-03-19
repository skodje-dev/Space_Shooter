using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _lives = 3;
    [SerializeField] private float _speed = 3.0f;
    private float _xMaxBounds = 10f;
    private float _xMinBounds = -10f;
    private float _yMaxBounds = 1f;
    private float _yMinBounds = -4f;

    [SerializeField] private GameObject _laserPrefab = default;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }
        
        var movement = GetInput();
        var velocity = movement * Time.deltaTime * _speed;
        transform.Translate(velocity);
        CheckPosition();
    }

    private static Vector3 GetInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        var movement = new Vector3(hInput, vInput);
        return movement;
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
}
