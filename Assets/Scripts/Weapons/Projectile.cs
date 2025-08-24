using System.Collections;
using UnityEngine;

/// <summary>
/// Управляет поведением одного снаряда: движением, временем жизни и столкновениями.
/// </summary>
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    // Поля, получаемые при инициализации
    private int _damage;
    private int _penetrationCount;
    private float _knockbackForce;
    private float _knockbackDuration;

    private bool _hasHitTarget;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Инициализирует снаряд с заданными параметрами после его получения из пула.
    /// </summary>
    public void Initialize(Vector2 direction, int damage, int penetration, float knockbackForce, float knockbackDuration, float speed, float lifetime, float sizeMultiplier)
    {
        _hasHitTarget = false;
        
        _damage = damage;
        _penetrationCount = penetration;
        _knockbackForce = knockbackForce;
        _knockbackDuration = knockbackDuration;

        _rigidbody.velocity = direction.normalized * speed;
        transform.up = direction;
        transform.localScale = Vector3.one * sizeMultiplier;

        StartCoroutine(ReturnToPoolAfterTime(lifetime));
    }

    private IEnumerator ReturnToPoolAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (!_hasHitTarget)
        {
            GameManager.Instance.AddProjectileMiss();
        }

        ObjectPool.Instance.ReturnToPool("Projectile", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            _hasHitTarget = true;
            
            if (_knockbackForce > 0)
            {
                Vector2 direction = (other.transform.position - transform.position).normalized;
                enemy.ApplyKnockback(direction, _knockbackForce, _knockbackDuration);
            }
            
            enemy.TakeDamage(_damage);
        }

        else if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            _hasHitTarget = true;
            damageable.TakeDamage(_damage);
        }
        
        _penetrationCount--;
        if (_penetrationCount <= 0)
        {
            StopAllCoroutines();
            ObjectPool.Instance.ReturnToPool("Projectile", gameObject);
        }
    }
}