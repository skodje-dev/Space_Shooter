using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField, Range(1, 3)] private int _lives = 3;
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _speedboostMultiplier = 2.0f;
    [SerializeField] private GameObject _laserPrefab = default;
    [SerializeField] private GameObject _tripleshotPrefab = default;
    [SerializeField] private float _laserSpawnYOffset = 0f;
    [SerializeField] private AudioClip _laserSoundClip;
    [SerializeField] private float _shootDelay = 0.4f;
    [SerializeField] private GameObject _shieldVisualizer = default;
    [SerializeField] private GameObject[] _playerHurtVisualizer;
    [SerializeField] private GameObject _thruster = default;
    [SerializeField] private bool _speedBoostActive = false;
    
    private WaitForSeconds _defaultPowerdownTime = new WaitForSeconds(5.0f);
    private float _canFire = 0f;
    private float _xMaxBounds = 9f;
    private float _xMinBounds = -9f;
    private float _yMaxBounds = .5f;
    private float _yMinBounds = -3f;
    private bool _tripleShotActive;
    private float _speedBoostTimer = 0.0f;
    private bool _hasShields = false;
    private int _shieldCharges = 1;

    private AudioSource _audio;
    private UIManager _uiManager = default;
    private SpawnManager _spawnManager = default;
    private int _score = 0;
    private int _previousDamage = -1;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _uiManager = FindObjectOfType<UIManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _thruster.SetActive(true);
    }

    private void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        var movement = GetInput();
        if (Input.GetKeyDown(KeyCode.Space) && _canFire < Time.time)
        {
            FireLaser();
        }

        _speedBoostActive = _speedBoostTimer > Time.time;
        var velocity = movement * Time.deltaTime * _speed * (_speedBoostActive ? _speedboostMultiplier : 1);
        transform.Translate(velocity);
        CheckPosition();
    }

    private void FireLaser()
    {
        _audio.PlayOneShot(_laserSoundClip);
        _canFire = Time.time + _shootDelay;
        if (!_tripleShotActive)
        {
            Vector3 laserSpawnPos = transform.position;
            laserSpawnPos.y += _laserSpawnYOffset;
            Instantiate(_laserPrefab, laserSpawnPos, Quaternion.identity);
        }
        else
        {
            Instantiate(_tripleshotPrefab, transform.position, Quaternion.identity);
        }
    }

    private static Vector3 GetInput()
    {
        var hInput = Input.GetAxis("Horizontal");
        var vInput = Input.GetAxis("Vertical");

        return new Vector3(hInput, vInput);
    }

    private void CheckPosition()
    {
        var position = transform.position;
        position = new Vector3(position.x, Mathf.Clamp(position.y, _yMinBounds, _yMaxBounds));
        
        if (transform.position.x < _xMinBounds)
            position = new Vector3(_xMaxBounds, position.y);
        if (position.x > _xMaxBounds)
            position = new Vector3(_xMinBounds, position.y);

        transform.position = position;
    }

    public void Damage()
    {
        if (_hasShields)
        {
            _shieldCharges--;
            if (_shieldCharges > 0) return;
            _hasShields = false;
            _shieldVisualizer.SetActive(_hasShields);
        }
        else
        {
            _lives--;
            if (_lives >= 0)
            {
                _uiManager.UpdateLivesDisplay(_lives);
                ShowDamage();
            }
            if (_lives <= 0)
            {
                _spawnManager.GameOver();
                _uiManager.GameOver();
                Destroy(gameObject);
            }
        }
    }
    
    private void ShowDamage()
    {
        int showDamage = Random.Range(0, 2);
        switch (_previousDamage)
        {
            case -1:
                _playerHurtVisualizer[showDamage].SetActive(true);
                break;
            case 0:
                _playerHurtVisualizer[1].SetActive(true); //right wing
                break;
            case 1:
                _playerHurtVisualizer[0].SetActive(true); //left wing
                break;
            default:
                Debug.Log("No damage to apply");
                break;
        }
        _previousDamage = showDamage;
    }

    private void FixDamage()
    {
        bool leftWingDamaged = _playerHurtVisualizer[0].activeSelf;
        bool rightWingDamaged = _playerHurtVisualizer[1].activeSelf;
        if (leftWingDamaged)
        {
            _playerHurtVisualizer[0].SetActive(false);
            return;
        }
        else if(rightWingDamaged)
        {
            _playerHurtVisualizer[1].SetActive(false);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            Destroy(other.gameObject);
            Damage();
        }

        if (other.TryGetComponent(out Powerup powerup))
        {
            ActivatePowerup(powerup.PowerupID);
            Destroy(powerup.gameObject);
        }
    }

    private void ActivatePowerup(int powerupID)
    {
        switch (powerupID)
        {
            case 0:
                StartCoroutine(TripleShotRoutine());
                break;
            case 1:
                ChargeSpeedBoost();
                break;
            case 2:
                EnableShields();
                break;
        }
    }

    private void EnableShields()
    {
        _hasShields = true;
        _shieldVisualizer.SetActive(_hasShields);
    }

    private IEnumerator TripleShotRoutine()
    {
        _tripleShotActive = true;
        yield return _defaultPowerdownTime;
        _tripleShotActive = false;
    }

    private void ChargeSpeedBoost()
    {
        if (_speedBoostTimer <= 0) _speedBoostTimer = Time.time + 5.0f;
        else _speedBoostTimer += 5.0f;
    }

    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScoreText(_score);
    }
}
