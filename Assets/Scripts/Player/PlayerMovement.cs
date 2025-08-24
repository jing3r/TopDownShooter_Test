using UnityEngine;

/// <summary>
/// Отвечает за перемещение игрока на основе ввода.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    [Tooltip("Скорость передвижения игрока в юнитах/сек.")]
    [SerializeField, Min(0f)] private float moveSpeed = 7f;

    private Rigidbody2D _rigidbody;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        // Применяем движение в FixedUpdate для стабильной физики
        _rigidbody.velocity = _moveInput.normalized * moveSpeed;
    }
}