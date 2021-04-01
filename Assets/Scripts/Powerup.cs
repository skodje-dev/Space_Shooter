using System;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [field: SerializeField, Min(0)] public int PowerupID { get; private set; } = 0;
    [SerializeField, Min(0.0f)] private float _speed = 3.0f;
    [SerializeField] private AudioClip _powerupCollectSound;
    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        if(transform.position.y < -8f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (AudioManager.Instance && other.GetComponent<Player>()) AudioManager.Instance.PlayAudioClip(_powerupCollectSound);
    }
}
