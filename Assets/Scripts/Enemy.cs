using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private bool _diagonalMovement = false;
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private GameObject _laserPrefab = default;
    [SerializeField] private int _killScore = 10;

    [Header("Fire Settings")]
    [SerializeField, Range(0.5f, 5.0f)] private float _shootDelay = 2.0f;
    [SerializeField, Range(0, 100)] private int _variancePct = 0;
    [SerializeField] private bool _canFireBackwards;
    private float _firstShotDelay = 1.0f;


    [SerializeField] private AudioClip _explosionSoundClip;
    [SerializeField] private AudioClip _laserSoundClip;
    private float _variance;
    private float _canFire;
    private bool _dead = false;
    private Animator _anim;
    private AudioSource _audio;
    private Player _player;
    private bool _fireAtPowerup;
    [SerializeField] private bool _evasiveManeuver;
    [SerializeField] private bool _hasShields;
    private GameObject _shield;
    
    

    private void Start()
    {
        if (_diagonalMovement)
        {
            int randomDiag = Random.Range(0, 2);
            transform.Rotate(transform.forward, randomDiag == 0 ? 45 : -45);
            _canFireBackwards = false;
        }
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _canFire = Time.time + _firstShotDelay;
        _variance = _shootDelay * _variancePct / 100;
        _player = FindObjectOfType<Player>();
        if (!GetComponent<Detector>())
        {
            _fireAtPowerup = false;
            _evasiveManeuver = false;
        }

        EnemyShield enemyShield = GetComponentInChildren<EnemyShield>();
        if (_hasShields && enemyShield) _shield = enemyShield.gameObject;
        else if (!_hasShields && enemyShield)
        {
            Destroy(enemyShield.gameObject);
        }

    }

    private void Update()
    {
        if (_dead) return;
        
        CalculateMovement();
        if (_fireAtPowerup && GetComponentInChildren<Detector>().CheckType("Powerup"))
        {
            FireLaser();
            _fireAtPowerup = false;
        }

        if (_evasiveManeuver && GetComponentInChildren<Detector>().CheckTag("Laser"))
        {
            StartCoroutine(EvasionProtocol());
        }
        if (_canFire < Time.time) FireLaser();
    }

    private IEnumerator EvasionProtocol()
    {
        float evasionDuration = Time.time + .1f;
        int evationDirection = Random.Range(0, 2);
        while (Time.time < evasionDuration)
        {
            yield return new WaitForEndOfFrame();
            transform.position += new Vector3(evationDirection == 1 ? -.001f :.001f, 0, 0);
        }
    }

    private void FireLaser()
    {
        var firebackwards = _canFireBackwards && _player.transform.position.y > transform.position.y;
        Vector3 laserPos = transform.position + new Vector3(0, firebackwards?3:0, 0);
        
        GameObject laserGO = Instantiate(_laserPrefab, laserPos, transform.rotation);
        Laser[] enemyLaser = laserGO.GetComponentsInChildren<Laser>();
        foreach (var laser in enemyLaser)
        {
            laser.tag = "EnemyLaser";
            if(!firebackwards)laser.SetEnemyLaser();
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            OnEnemyDeath();
            Destroy(other.gameObject);
            
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
        if (_hasShields)
        {
            Destroy(_shield);
            _hasShields = false;
            return;
        }
        if (_anim) _anim.SetTrigger("Destroy");
        _dead = true;
        GetComponent<AudioSource>().PlayOneShot(_explosionSoundClip);
        Destroy(GetComponent<Collider2D>());
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }
}
