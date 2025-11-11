using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Game : MonoBehaviour
{
    private const float MinimumSpawnDelay = 0.001f;

    [Header("Prefabs")]
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Player _playerPrefab;

    [Header("Game Objects")]
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private Camera _gameCamera;

    [Header("Spawn Settings")]
    [SerializeField, Min(1)] private int _enemyCount = 1000;
    [SerializeField] private Vector2 _enemySpawnCenter = Vector2.zero;
    [SerializeField, Min(0f)] private float _spawnRadius = 10f;
    [SerializeField] private Vector2 _playerSpawnPosition = new Vector2(2f, 0f);
    [SerializeField] private bool _spawnInstantly = true;
    [SerializeField, Min(0f)] private float _spawnDelay = 0.01f;

    [Header("Enemy Behaviour")]
    [SerializeField, Min(0f)] private float _enemyScreenEdgeMargin = 30f;

    private readonly List<Enemy> _spawnedEnemies = new List<Enemy>();
    private Player _playerInstance;
    private Coroutine _spawnCoroutine;
    [SerializeField]
    private Button _backButton;

    private void OnEnable()
    {
        StartGame();
        _backButton.Select();
    }

    private void OnDisable()
    {
        StopSpawning();
        CleanupSpawnedEntities();
    }

    private void StartGame()
    {
        ValidateDependencies();
        CleanupSpawnedEntities();
        SpawnPlayer();

        if (_spawnInstantly)
        {
            SpawnEnemiesInstant();
        }
        else
        {
            _spawnCoroutine = StartCoroutine(SpawnEnemiesGradually());
        }
    }

    private void ValidateDependencies()
    {
        if (_gameCamera == null)
        {
            _gameCamera = Camera.main;
        }
    }

    private IEnumerator SpawnEnemiesGradually()
    {
        var delay = Mathf.Max(_spawnDelay, MinimumSpawnDelay);

        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemyInstance();
            yield return new WaitForSecondsRealtime(delay);
        }

        _spawnCoroutine = null;
    }

    private void SpawnEnemiesInstant()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemyInstance();
        }
    }

    private void SpawnEnemyInstance()
    {
        if (_enemyPrefab == null || _playerInstance == null)
        {
            return;
        }

        Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
        Vector2 spawnPos = _enemySpawnCenter + randomOffset;

        Enemy enemyInstance = Instantiate(_enemyPrefab, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);
        enemyInstance.Initialize(
            _playerInstance.transform,
            _gameCamera,
            _enemyScreenEdgeMargin);

        _spawnedEnemies.Add(enemyInstance);
    }

    private void SpawnPlayer()
    {
        if (_playerPrefab == null)
        {
            return;
        }

        _playerInstance = Instantiate(_playerPrefab, new Vector3(_playerSpawnPosition.x, _playerSpawnPosition.y, 0f), Quaternion.identity);
        _playerInstance.Initialize(_gameCamera);
    }

    private void StopSpawning()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private void CleanupSpawnedEntities()
    {
        foreach (Enemy enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        _spawnedEnemies.Clear();

        if (_playerInstance != null)
        {
            Destroy(_playerInstance.gameObject);
            _playerInstance = null;
        }
    }

    public void BackClick()
    {
        StopSpawning();
        CleanupSpawnedEntities();

        if (_gameCamera != null)
        {
            _gameCamera.backgroundColor = Color.blue;
        }

        if (_menuCanvas != null)
        {
            _menuCanvas.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
