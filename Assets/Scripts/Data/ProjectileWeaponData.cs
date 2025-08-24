using UnityEngine;

/// <summary>
/// ScriptableObject для оружия, стреляющего снарядами. Наследуется от WeaponData.
/// </summary>
[CreateAssetMenu(fileName = "NewProjectileWeaponData", menuName = "Data/Projectile Weapon Data")]
public class ProjectileWeaponData : WeaponData
{
    [Header("Визуализация")]
    [Tooltip("Иконка, представляющая это оружие в UI.")]
    public Sprite weaponIcon;

    [Header("Параметры выстрела")]
    [Tooltip("Префаб снаряда, который будет создан.")]
    public Projectile projectilePrefab;
    [Tooltip("Количество снарядов, выпускаемых за один выстрел.")]
    [Range(1, 20)] public int numberOfProjectiles = 1;
    [Tooltip("Угол разброса между снарядами (для дробовика).")]
    [Range(0f, 90f)] public float spreadAngle = 0f;

    [Header("Характеристики снаряда")]
    [Tooltip("Урон, наносимый каждым снарядом.")]
    [Min(1)] public int damage = 1;
    [Tooltip("Скорость полета снаряда в юнитах/сек.")]
    [Min(0f)] public float projectileSpeed = 20f;
    [Tooltip("Время жизни снаряда в секундах. Определяет дальность.")]
    [Min(0.1f)] public float projectileLifetime = 2f;
    [Tooltip("Множитель размера снаряда. 1 = базовый размер.")]
    [Range(0.2f, 3f)] public float projectileSizeMultiplier = 1f;
    [Tooltip("Сколько врагов может пробить снаряд перед исчезновением.")]
    [Min(1)] public int penetrationCount = 1;
    [Tooltip("Сила, с которой снаряд отталкивает врага.")]
    [Min(0f)] public float knockbackForce = 0f;
    [Tooltip("Длительность оглушения врага после отталкивания (в секундах).")]
    [Min(0f)] public float knockbackDuration = 0.5f;
}