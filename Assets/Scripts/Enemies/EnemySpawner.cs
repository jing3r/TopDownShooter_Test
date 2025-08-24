using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет процессом появления врагов в игре.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спауна")]
    [Tooltip("Префаб врага, который будет использоваться для создания.")]
    [SerializeField] private EnemyController enemyPrefab;
    [Tooltip("Список типов врагов (ScriptableObjects), которые могут появиться.")]
    [SerializeField] private List<EnemyData> enemyTypes = new List<EnemyData>();
    
    [Header("Параметры волн")]
    [Tooltip("Интервал в секундах между волнами спауна.")]
    [SerializeField, Min(0.1f)] private float spawnInterval = 3f;
    [Tooltip("Радиус вокруг игрока, в котором появляются враги.")]
    [SerializeField, Min(1f)] private float spawnRadius = 15f;
    [Tooltip("Количество врагов, создаваемых за одну волну.")]
    [SerializeField, Min(1)] private int enemiesPerWave = 3;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = ServiceLocator.Get<Transform>();

        if (_playerTransform == null)
        {
            Debug.LogError("Спаунер не нашел игрока через ServiceLocator. Компонент будет отключен.", this);
            enabled = false;
            return;
        }
        
        if (enemyPrefab == null || enemyTypes.Count == 0)
        {
            Debug.LogError("В спаунере не назначен префаб врага или типы врагов. Компонент будет отключен.", this);
            enabled = false;
            return;
        }

        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        // Бесконечный цикл создания волн
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnSingleEnemy();
            }
        }
    }

    private void SpawnSingleEnemy()
    {
        EnemyData randomEnemyData = enemyTypes[Random.Range(0, enemyTypes.Count)];

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = _playerTransform.position + (Vector3)(randomDirection * spawnRadius);

        GameObject enemyObject = ObjectPool.Instance.SpawnFromPool("Enemy", spawnPosition, Quaternion.identity);
        
        if (enemyObject != null)
        {
            EnemyController newEnemy = enemyObject.GetComponent<EnemyController>();
            newEnemy.Initialize(randomEnemyData);
        }
    }
}