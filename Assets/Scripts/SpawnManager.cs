using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _timeBeforeFirstSpawn = 3.0f;
    [SerializeField] private float _minSpawnDelay = 2.0f;
    [SerializeField] private float _maxSpawnDelay = 5.0f;

    private bool _gameNotOver = true;
    
    void Start()
    {
        if (_enemyPrefab) StartCoroutine(EnemySpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeFirstSpawn);
        while (_gameNotOver)
        {
            Instantiate(_enemyPrefab);
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));
        }
    }

    void Update()
    {
        
    }
}
