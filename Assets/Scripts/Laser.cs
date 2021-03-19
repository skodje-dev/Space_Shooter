using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField, Tooltip("Laser will travel upwards based on rotation")] private float _speed = 7.0f;
    void Start()
    {
        
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.position += transform.up * Time.deltaTime * _speed;
        if (Mathf.Abs(transform.position.y) > 8.0f)
        {
            Destroy(gameObject);
        }
    }
}
