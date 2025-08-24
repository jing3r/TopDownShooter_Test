using UnityEngine;

/// <summary>
/// Реализация оружия, стреляющего снарядами.
/// </summary>
public class ProjectileWeapon : WeaponBase
{
    private ProjectileWeaponData _projectileWeaponData;

    private void Awake()
    {
        // Приведение базовых данных к конкретному типу
        _projectileWeaponData = weaponData as ProjectileWeaponData;
        
        if (_projectileWeaponData == null)
        {
            Debug.LogError("На оружии назначены неверные данные! Ожидается ProjectileWeaponData.", this);
            enabled = false;
        }
    }
    
    /// <summary>
    /// Создает один или несколько снарядов в соответствии с данными оружия.
    /// </summary>
    public override void Attack()
    {
        if (_projectileWeaponData.projectilePrefab == null) return;
        
        float startAngle = 0;
        // Рассчитываем стартовый угол только если снарядов больше одного
        if (_projectileWeaponData.numberOfProjectiles > 1)
        {
            startAngle = -(_projectileWeaponData.numberOfProjectiles - 1) * _projectileWeaponData.spreadAngle / 2f;
        }

        for (int i = 0; i < _projectileWeaponData.numberOfProjectiles; i++)
        {
            GameManager.Instance.AddProjectileFired();
            
            float angle = startAngle + i * _projectileWeaponData.spreadAngle;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector2 direction = rotation * firePoint.up;

            GameObject projectileObject = ObjectPool.Instance.SpawnFromPool("Projectile", firePoint.position, firePoint.rotation);

            if (projectileObject != null)
            {
                Projectile projectile = projectileObject.GetComponent<Projectile>();
                projectile.Initialize(
                    direction,
                    _projectileWeaponData.damage,
                    _projectileWeaponData.penetrationCount, 
                    _projectileWeaponData.knockbackForce,
                    _projectileWeaponData.knockbackDuration,
                    _projectileWeaponData.projectileSpeed,
                    _projectileWeaponData.projectileLifetime,
                    _projectileWeaponData.projectileSizeMultiplier
                );
            }
        }
    }

    /// <summary>
    /// Возвращает данные этого оружия. Используется внешними системами (например, UI).
    /// </summary>
    public WeaponData GetWeaponData() 
    {
        return weaponData;
    }
}