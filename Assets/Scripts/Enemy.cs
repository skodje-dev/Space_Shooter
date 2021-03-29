using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab = default;
    [SerializeField] private int _killScore = 10;

    [Header("Fire Settings")]
    private float _firstShotDelay = 1.0f;
    [SerializeField, Range(0.5f, 5.0f)] private float _shootDelay = 2.0f;
    [SerializeField, Range(0, 100)] private int _variancePct = 0;
    private float _variance;

    private float _canFire;
    private void Start()
    {
        _canFire = Time.time + _firstShotDelay;
        _variance = _shootDelay * _variancePct / 100;
    }

    private void Update()
    {
        CalculateMovement();
        if (_canFire < Time.time) FireLaser();
    }

    private void FireLaser()
    {
        GameObject laserGO = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        Laser[] enemyLaser = laserGO.GetComponentsInChildren<Laser>();
        foreach (var laser in enemyLaser)
        {
            laser.tag = "EnemyLaser";
            laser.SetEnemyLaser();
        }

        if (_variancePct == 0)
            _canFire = Time.time + _shootDelay;
        else
            _canFire = Time.time + Random.Range(_shootDelay - _variance, _shootDelay + _variance);
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -8f)
        {
            float randomXPos = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomXPos, 8f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Player _player = FindObjectOfType<Player>();
            _player.AddScore(_killScore);
            Destroy(gameObject);
        }

        if (other.TryGetComponent(out Player player))
        {
            player.Damage();
            Destroy(gameObject);
        }
    }
}
