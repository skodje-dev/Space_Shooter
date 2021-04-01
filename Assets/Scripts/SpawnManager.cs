using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float _timeBeforeFirstSpawn = 3.0f;
    [SerializeField] private float _minSpawnDelay = 2.0f;
    [SerializeField] private float _maxSpawnDelay = 5.0f;
    [Space, Header("Spawn")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;
    [Space] [SerializeField] private GameObject[] _powerupPrefabs;

    private bool _gameOver;
    
    public void StartSpawning()
    {
        if (_enemyPrefab) StartCoroutine(EnemySpawnRoutine());
        if (_powerupPrefabs.Length > 0) StartCoroutine(PowerupSpawnRoutine());
    }

    private IEnumerator PowerupSpawnRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeFirstSpawn);
        while (!_gameOver)
        {
            int randomPowerup = Random.Range(0, _powerupPrefabs.Length);
            Instantiate(_powerupPrefabs[randomPowerup], GetRandomSpawnPosition(), Quaternion.identity, _enemyContainer);
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
        }
    }

    private IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeFirstSpawn);
        while (!_gameOver)
        {
            Instantiate(_enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity, _enemyContainer);
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomXPos = Random.Range(-9.0f, 9.0f);
        return new Vector3(randomXPos, 8f, 0);
    }

    public void GameOver()
    {
        _gameOver = true;
        StopCoroutine(EnemySpawnRoutine());
        Destroy(_enemyContainer.gameObject);
    }
}
