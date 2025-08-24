using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Основные компоненты")]
    [Tooltip("ScriptableObject с характеристиками этого оружия.")]
    [SerializeField] protected WeaponData weaponData;
    [Tooltip("Точка, из которой производятся выстрелы.")]
    [SerializeField] protected Transform firePoint;

    /// <summary>
    /// Основное действие оружия.
    /// </summary>
    public abstract void Attack();
    
    /// <summary>
    /// Возвращает задержку между выстрелами в секундах.
    /// </summary>
    public float GetFireRateDelay()
    {
        // fireRate > 0, чтобы избежать деления на ноль.
        return weaponData.fireRate > 0 ? 1f / weaponData.fireRate : float.MaxValue;
    }
}