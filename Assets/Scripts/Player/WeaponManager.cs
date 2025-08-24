using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет арсеналом игрока и переключением между видами оружия.
/// </summary>
public class WeaponManager : MonoBehaviour
{
    [Header("Оружие")]
    [Tooltip("Список всех доступных объектов оружия (дочерние объекты игрока).")]
    [SerializeField] private List<WeaponBase> weapons;
    
    [Header("Визуализация")]
    [Tooltip("Sprite Renderer для отображения иконки текущего оружия.")]
    [SerializeField] private SpriteRenderer weaponIconRenderer;

    public WeaponBase CurrentWeapon { get; private set; }
    private int _currentWeaponIndex = -1;

    private void Start()
    {
        // Деактивируем все оружие при старте для чистоты
        foreach (var weapon in weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        
        if (weapons.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    private void Update()
    {
        HandleWeaponSwitching();
    }
    
    private void HandleWeaponSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(2);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            int nextIndex = (_currentWeaponIndex + 1) % weapons.Count;
            EquipWeapon(nextIndex);
        }
        else if (scroll < 0f)
        {
            int prevIndex = (_currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
            EquipWeapon(prevIndex);
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count || index == _currentWeaponIndex)
        {
            return;
        }
        
        if (CurrentWeapon != null)
        {
            CurrentWeapon.gameObject.SetActive(false);
        }

        _currentWeaponIndex = index;
        CurrentWeapon = weapons[_currentWeaponIndex];
        CurrentWeapon.gameObject.SetActive(true);

        UpdateWeaponIcon();
    }
    
    private void UpdateWeaponIcon()
    {
        if (weaponIconRenderer == null) return;
        
        var projectileWeapon = CurrentWeapon.GetComponent<ProjectileWeapon>();
        if (projectileWeapon != null)
        {
            var projectileWeaponData = projectileWeapon.GetWeaponData() as ProjectileWeaponData;
            if (projectileWeaponData != null && projectileWeaponData.weaponIcon != null)
            {
                weaponIconRenderer.sprite = projectileWeaponData.weaponIcon;
                // Иконка снаряда всегда видима, ее готовность показывает PlayerShooting
            }
        }
    }
}