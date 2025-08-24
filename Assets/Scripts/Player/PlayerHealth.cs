using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет здоровьем игрока и обработкой столкновений с врагами.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Параметры здоровья")]
    [Tooltip("Максимальный запас здоровья.")]
    [SerializeField, Min(1)] private int maxHealth = 5;
    
    [Header("UI")]
    [Tooltip("Ссылка на UI Image для отображения здоровья.")]
    [SerializeField] private Image healthBarImage;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealthUI();
    }
    
    /// <summary>
    /// Применяет урон к игроку.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        UpdateHealthUI();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); // Урон от столкновения пока захардкожен
            ObjectPool.Instance.ReturnToPool("Enemy", collision.gameObject);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarImage != null)
        {
            float fill = (float)_currentHealth / maxHealth;
            healthBarImage.fillAmount = Mathf.Clamp01(fill);
        }
    }

    private void Die()
    {
        // Убеждаемся, что не вызываем Die() повторно
        if (!enabled) return;
        
        Debug.Log("Игрок побежден! Игра окончена.");
        GameManager.Instance.OnPlayerDied();
        enabled = false; // Отключаем компонент, чтобы предотвратить повторные вызовы
    }
}