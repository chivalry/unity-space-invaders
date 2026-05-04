using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads horizontal input from the New Input System and moves the player ship,
/// clamping its position within the visible play field.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>Horizontal movement speed in units per second.</summary>
    [SerializeField] private float moveSpeed = 8f;

    /// <summary>Absolute X limit that clamps the player within the screen.</summary>
    [SerializeField] private float screenHalfWidth = 8.5f;

    private InputSystem_Actions _input;

    /// <summary>Creates the generated input actions asset.</summary>
    void Awake()
    {
        _input = new InputSystem_Actions();
    }

    /// <summary>Enables the Player action map.</summary>
    void OnEnable()
    {
        _input.Player.Enable();
    }

    /// <summary>Disables the Player action map.</summary>
    void OnDisable()
    {
        _input.Player.Disable();
    }

    /// <summary>
    /// Reads the horizontal Move axis and applies clamped movement to the transform.
    /// </summary>
    void Update()
    {
        float moveX = _input.Player.Move.ReadValue<Vector2>().x;

        Vector3 pos = transform.position;
        pos.x += moveX * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -screenHalfWidth, screenHalfWidth);
        transform.position = pos;
    }
}
