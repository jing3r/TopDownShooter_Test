using UnityEngine;

/// <summary>
/// Базовый ScriptableObject для всех типов оружия. Содержит общие параметры.
/// </summary>
[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Общие параметры")]
    [Tooltip("Название оружия для UI или отладки.")]
    public string weaponName = "Default Weapon";
    
    [Tooltip("Выстрелов в секунду.")]
    [Range(0.1f, 30f)] public float fireRate = 5f; 
}