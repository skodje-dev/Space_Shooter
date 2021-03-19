using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    void Start()
    {
        
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -8f)
        {
            float randX = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randX, 8f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Laser>())
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
