using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private bool _diagonalMovement = false;
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab = default;
    [SerializeField] private int _killScore = 10;

    [Header("Fire Settings")]
    private float _firstShotDelay = 1.0f;
    [SerializeField, Range(0.5f, 5.0f)] private float _shootDelay = 2.0f;
    [SerializeField, Range(0, 100)] private int _variancePct = 0;

    [SerializeField] private AudioClip _explosionSoundClip;
    [SerializeField] private AudioClip _laserSoundClip;
    private float _variance;
    private float _canFire;
    private bool _dead = false;
    private Animator _anim;
    private AudioSource _audio;

    private void Start()
    {
        if (_diagonalMovement)
        {
            int randomDiag = Random.Range(0, 2);
            transform.Rotate(transform.forward, randomDiag == 0 ? 45 : -45);
        }
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _canFire = Time.time + _firstShotDelay;
        _variance = _shootDelay * _variancePct / 100;
    }

    private void Update()
    {
        CalculateMovement();
        if (!_dead && _canFire < Time.time) FireLaser();
    }

    private void FireLaser()
    {
        GameObject laserGO = Instantiate(_laserPrefab, transform.position, transform.rotation);
        Laser[] enemyLaser = laserGO.GetComponentsInChildren<Laser>();
        foreach (var laser in enemyLaser)
        {
            laser.tag = "EnemyLaser";
            laser.SetEnemyLaser();
        }
        _audio.PlayOneShot(_laserSoundClip);

        if (_variancePct == 0)
            _canFire = Time.time + _shootDelay;
        else
            _canFire = Time.time + Random.Range(_shootDelay - _variance, _shootDelay + _variance);
    }

    private void CalculateMovement()
    {
        transform.position += -transform.up * Time.deltaTime * _speed;

        if (transform.position.y < -8f || _diagonalMovement && transform.position.x < -11f || _diagonalMovement && transform.position.x > 11f)
        {
            float randomXPos = Random.Range(-9.0f, 9.0f);
            transform.position = new Vector3(randomXPos, 8f, 0);
        }
    }

    private void AlternateMovement()
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
        _anim.SetTrigger("Destroy");
        _dead = true;
        _speed = 0;
        GetComponent<AudioSource>().PlayOneShot(_explosionSoundClip);
        Destroy(GetComponent<Collider2D>());
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }
}
