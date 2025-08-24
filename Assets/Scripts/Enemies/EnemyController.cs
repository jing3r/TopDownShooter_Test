using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Управляет поведением, состоянием и визуализацией одного врага.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("UI")]
    [Tooltip("Ссылка на UI Image, который будет служить шкалой здоровья.")]
    [SerializeField] private Image healthBarImage;

    // Компоненты
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;
    
    // Данные и состояние
    private EnemyData _enemyData;
    private Transform _playerTransform;
    private int _currentHealth;
    private float _initialColliderRadius;
    private bool _isKnockedBack;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        
        if (_collider != null)
        {
            _initialColliderRadius = _collider.radius;
        }
    }

    private void OnDisable()
    {
        // Сбрасываем состояние при возврате в пул для предотвращения багов
        StopAllCoroutines();
        _isKnockedBack = false;
    }

    /// <summary>
    /// Инициализирует врага данными из ScriptableObject. Вызывается из пула объектов.
    /// </summary>
    public void Initialize(EnemyData data)
    {
        // Принудительный сброс состояния перед пересозданием из пула
        _isKnockedBack = false;
        _rigidbody.velocity = Vector2.zero;
        
        _enemyData = data;
        _currentHealth = _enemyData.maxHealth;
        
        _spriteRenderer.color = _enemyData.enemyColor;
        transform.localScale = Vector3.one * _enemyData.sizeMultiplier;
        if (_collider != null)
        {
            _collider.radius = _initialColliderRadius * _enemyData.sizeMultiplier;
        }

        if (_playerTransform == null)
        {
            _playerTransform = ServiceLocator.Get<Transform>();
        }
        
        UpdateHealthUI();
    }

    private void FixedUpdate()
    {
        if (!_isKnockedBack)
        {
            HandleMovement();
        }
    }

    private void Update()
    {
        HandleRotation();
    }

    /// <summary>
    /// Применяет урон к врагу и обновляет его состояние.
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
    
    /// <summary>
    /// Применяет к врагу силу отталкивания и временно оглушает его.
    /// </summary>
    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (_isKnockedBack || force <= 0) return;

        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        StartCoroutine(KnockbackRoutine(duration));
    }

    private void HandleMovement()
    {
        if (_playerTransform != null && _enemyData != null)
        {
            Vector2 direction = (_playerTransform.position - transform.position).normalized;
            _rigidbody.velocity = direction * _enemyData.moveSpeed;
        }
    }

    private void HandleRotation()
    {
        if (_rigidbody.velocity.sqrMagnitude > 0.01f)
        {
            transform.right = _rigidbody.velocity.normalized;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarImage != null)
        {
            // Используем Clamp01 для гарантии, что значение будет между 0 и 1
            float fill = (float)_currentHealth / _enemyData.maxHealth;
            healthBarImage.fillAmount = Mathf.Clamp01(fill);
        }
    }

    private IEnumerator KnockbackRoutine(float duration)
    {
        _isKnockedBack = true;
        yield return new WaitForSeconds(duration);
        _isKnockedBack = false;
    }
    
    private void Die()
    {
        GameManager.Instance.AddEnemyKill();
        ObjectPool.Instance.ReturnToPool("Enemy", gameObject);
    }
}