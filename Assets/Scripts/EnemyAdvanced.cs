using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAdvanced : MonoBehaviour
{
    [SerializeField] private bool _targetPickups;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private int _killScore = 25;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioClip _laserSoundClip;
    private const float FirstShotDelay = 1.0f;
    [SerializeField, Range(0.5f, 5.0f)] private float _shootDelay = 2.0f;
    private float _canFire;
    private AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _canFire = Time.time + FirstShotDelay;
    }

    void Update()
    {
        CalculateMovement();
        if(Time.time > _canFire) FireLaser();
    }

    
    private void FireLaser()
    {
        GameObject laserGO = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        laserGO.GetComponentInChildren<Laser>().SetEnemyLaser();
        _audio.PlayOneShot(_laserSoundClip);

        _canFire = Time.time + _shootDelay;
    }
    
    private void CalculateMovement()
    {
        Vector3 direction = -transform.up;
        direction.x +=  Mathf.Sin(Time.time) * 1.33f;
        transform.Translate(direction * Time.deltaTime * _speed);

        if (transform.position.y < -8f || transform.position.x < -11f || transform.position.x > 11f)
        {
            float randomXPos = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomXPos, 8f, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            OnEnemyDeath();
            Destroy(other.gameObject);
            Player _player = FindObjectOfType<Player>();
            _player.AddScore(_killScore);
        }

        if (other.TryGetComponent(out Player player))
        {
            player.Damage();
            OnEnemyDeath();
        }
    }
    
    private void OnEnemyDeath()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
