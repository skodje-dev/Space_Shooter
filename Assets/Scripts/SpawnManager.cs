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

    private static bool _gameOver;
    
    void Start()
    {
        if (_enemyPrefab) StartCoroutine(EnemySpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeFirstSpawn);
        while (!_gameOver)
        {
            Instantiate(_enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity, _enemyContainer);
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
        }
        Destroy(_enemyContainer.gameObject);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomXPos = Random.Range(-9.0f, 9.0f);
        return new Vector3(randomXPos, 8f, 0);
    }

    public static void GameOver()
    {
        _gameOver = true;
    }
}
