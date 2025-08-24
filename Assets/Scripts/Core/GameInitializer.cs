using UnityEngine;

/// <summary>
/// Инициализирует ключевые сервисы на старте игры.
/// Должен выполняться раньше других скриптов (см. Script Execution Order).
/// </summary>
public class GameInitializer : MonoBehaviour
{
    [Header("Ссылки на сервисы")]
    [Tooltip("Ссылка на префаб или объект игрока на сцене.")]
    [SerializeField] private PlayerHealth playerHealth;

    private void Awake()
    {
        ServiceLocator.Clear();
        
        if (playerHealth != null)
        {
            ServiceLocator.Register(playerHealth);
            ServiceLocator.Register(playerHealth.transform);
        }
        else
        {
            Debug.LogError("Ссылка на игрока (PlayerHealth) не установлена в GameInitializer!", this);
        }
    }
}