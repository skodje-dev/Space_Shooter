using UnityEngine;

public class Powerup : MonoBehaviour
{
    [field: SerializeField, Min(0)] public int PowerupID { get; private set; } = 0;
    [SerializeField, Min(0.0f)] private float _speed = 3.0f;
    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if(transform.position.y < -8f) Destroy(gameObject);
    }
}
