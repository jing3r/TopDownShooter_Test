/// <summary>
/// Определяет контракт для всех объектов, которые могут получать урон.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Применяет урон к объекту.
    /// </summary>
    /// <param name="damageAmount">Количество наносимого урона.</param>
    void TakeDamage(int damageAmount);
}