using UnityEngine;

/// <summary>
/// Обрабатывает ввод для стрельбы и управляет перезарядкой.
/// </summary>
[RequireComponent(typeof(WeaponManager))]
public class PlayerShooting : MonoBehaviour
{
    [Header("Визуализация")]
    [Tooltip("Sprite Renderer для отображения иконки готовности к выстрелу.")]
    [SerializeField] private SpriteRenderer weaponIconRenderer;

    private WeaponManager _weaponManager;
    private float _nextFireTime;

    private void Awake()
    {
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        HandleCooldownVisuals();

        if (Input.GetMouseButton(0))
        {
            TryShoot();
        }
    }

    private void HandleCooldownVisuals()
    {
        if (weaponIconRenderer != null)
        {
            bool canShoot = Time.time >= _nextFireTime;
            weaponIconRenderer.enabled = canShoot;
        }
    }
    
    private void TryShoot()
    {
        if (_weaponManager.CurrentWeapon != null && Time.time >= _nextFireTime)
        {
            _weaponManager.CurrentWeapon.Attack();
            _nextFireTime = Time.time + _weaponManager.CurrentWeapon.GetFireRateDelay();
        }
    }
}