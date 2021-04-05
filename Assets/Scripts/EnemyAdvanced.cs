using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvanced : MonoBehaviour
{
    [SerializeField] private float _speed = 5.0f;
    void Start()
    {
        
    }

    void Update()
    {
        CalculateMovement();
    }
    private void CalculateMovement()
    {
        Vector3 direction = -transform.up;
        direction.x +=  Mathf.Sin(Time.time) * 0.33f;
        transform.Translate(direction * Time.deltaTime * _speed);

        if (transform.position.y < -8f)
        {
            float randomXPos = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomXPos, 8f, 0);
        }
    }
}
