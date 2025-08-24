using UnityEngine;

/// <summary>
/// Отвечает за поворот игрока в сторону курсора мыши.
/// </summary>
public class PlayerAiming : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            Debug.LogError("На сцене нет основной камеры (с тегом 'MainCamera'). Прицеливание не будет работать.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        if (_mainCamera == null) return;

        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector2 direction = (Vector2)(mouseWorldPosition - transform.position);

        // Поворачиваем transform.right (ось X) в направлении курсора.
        // Это работает, если спрайт по умолчанию направлен вправо.
        transform.right = direction;
    }
}