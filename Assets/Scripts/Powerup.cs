using UnityEngine;

public class Powerup : MonoBehaviour
{
    [field: SerializeField, Min(0)] public int PowerupID { get; private set; } = 0;
    [field: SerializeField, Min(0.0f)] public float Speed { get; private set; } = 3.0f;
    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * Speed);
        if(transform.position.y < -8f) Destroy(gameObject);
    }
}
