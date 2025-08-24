using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Центральный класс, управляющий игровым циклом, сбором статистики и UI.
/// Реализован как синглтон для глобального доступа.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Элементы")]
    [Tooltip("Панель, отображаемая при окончании игры.")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI projectilesFiredText;
    [SerializeField] private TextMeshProUGUI projectilesHitText;
    [SerializeField] private TextMeshProUGUI projectilesMissedText;
    
    // Статистика сессии
    private int _enemiesKilled;
    private int _projectilesFired;
    private int _projectilesMissed;
    
    // Динамически вычисляемые свойства для статистики
    private int ProjectilesHit => _projectilesFired - _projectilesMissed;
    private float Accuracy => _projectilesFired > 0 ? (float)ProjectilesHit / _projectilesFired : 0f;

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
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
        UpdateStatsUI();
    }
    
    /// <summary>
    /// Регистрирует убийство врага и обновляет UI.
    /// </summary>
    public void AddEnemyKill()
    {
        _enemiesKilled++;
        UpdateStatsUI();
    }

    /// <summary>
    /// Регистрирует выпущенный снаряд и обновляет UI.
    /// </summary>
    public void AddProjectileFired()
    {
        _projectilesFired++;
        UpdateStatsUI();
    }

    /// <summary>
    /// Регистрирует промах снаряда (снаряд исчез, не попав ни в одну цель).
    /// </summary>
    public void AddProjectileMiss()
    {
        _projectilesMissed++;
        UpdateStatsUI();
    }
    
    /// <summary>
    /// Запускает процедуру окончания игры.
    /// </summary>
    public void OnPlayerDied()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Перезапускает текущую сцену.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void UpdateStatsUI()
    {
        killsText.text = $"Killed: {_enemiesKilled}";
        projectilesFiredText.text = $"Fired: {_projectilesFired}";
        projectilesHitText.text = $"Hit: {ProjectilesHit}";
        projectilesMissedText.text = $"Missed: {_projectilesMissed}";
        accuracyText.text = $"Accuracy: {Accuracy:P0}";
    }
}