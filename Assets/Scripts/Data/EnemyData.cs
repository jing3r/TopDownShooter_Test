using UnityEngine;

/// <summary>
/// ScriptableObject, содержащий все настраиваемые параметры для типа врага.
/// </summary>
[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Боевые параметры")]
    [Tooltip("Максимальный запас здоровья.")]
    [Min(1)] public int maxHealth = 10;
    [Tooltip("Скорость передвижения в юнитах/сек.")]
    [Min(0f)] public float moveSpeed = 3f;
    
    [Header("Визуальные параметры")]
    [Tooltip("Цвет спрайта врага.")]
    public Color enemyColor = Color.white;
    
    [Tooltip("Множитель размера. 1 = базовый размер.")]
    [Range(0.5f, 3f)] public float sizeMultiplier = 1f;
}