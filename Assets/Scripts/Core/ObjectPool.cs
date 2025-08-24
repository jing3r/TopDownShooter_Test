using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет пулами переиспользуемых игровых объектов для оптимизации производительности.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [Header("Определения пулов")]
    [Tooltip("Список пулов, которые будут созданы при старте.")]
    [SerializeField] private List<Pool> pools;

    private Dictionary<string, Queue<GameObject>> _poolDictionary;
    private Dictionary<string, Pool> _poolInfoDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        _poolInfoDictionary = new Dictionary<string, Pool>();

        foreach (Pool pool in pools)
        {
            _poolInfoDictionary.Add(pool.tag, pool);
            
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform); // Добавил transform
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            _poolDictionary.Add(pool.tag, objectQueue);
        }
    }

    /// <summary>
    /// Запрашивает объект из пула, активирует и размещает его в указанной позиции.
    /// Динамически расширяет пул, если свободных объектов нет.
    /// </summary>
    /// <returns>Активированный игровой объект или null, если пул не найден.</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Пул с тегом '{tag}' не существует.");
            return null;
        }
        
        GameObject objectToSpawn;

        if (_poolDictionary[tag].Count > 0)
        {
            objectToSpawn = _poolDictionary[tag].Dequeue();
        }
        else
        {
            Debug.LogWarning($"Пул с тегом '{tag}' пуст. Создается новый экземпляр.");
            Pool poolInfo = _poolInfoDictionary[tag];
            objectToSpawn = Instantiate(poolInfo.prefab, transform); // Добавил transform
        }

        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    /// <summary>
    /// Возвращает объект в пул, деактивируя его.
    /// </summary>
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Пул с тегом '{tag}' не найден. Объект будет уничтожен.");
            Destroy(objectToReturn);
            return;
        }
        
        objectToReturn.SetActive(false);
        _poolDictionary[tag].Enqueue(objectToReturn);
    }
}