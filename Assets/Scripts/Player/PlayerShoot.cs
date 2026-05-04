using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player shooting via the New Input System. Enforces a fire-rate cooldown
/// and spawns bullet prefabs slightly above the ship's position.
/// </summary>
public class PlayerShoot : MonoBehaviour
{
    /// <summary>Bullet prefab instantiated on each shot.</summary>
    [SerializeField] private GameObject bulletPrefab;

    /// <summary>Minimum seconds between shots.</summary>
    [SerializeField] private float fireRate = 0.5f;

    private InputSystem_Actions _input;
    private float _nextFireTime;

    /// <summary>Creates the generated input actions asset.</summary>
    void Awake()
    {
        _input = new InputSystem_Actions();
    }

    /// <summary>Enables the Player action map and subscribes to the Attack action.</summary>
    void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Attack.performed += OnFire;
    }

    /// <summary>Unsubscribes from the Attack action and disables the Player action map.</summary>
    void OnDisable()
    {
        _input.Player.Attack.performed -= OnFire;
        _input.Player.Disable();
    }

    /// <summary>
    /// Fires a bullet if the fire-rate cooldown has elapsed. Plays the shoot sound
    /// and instantiates <see cref="bulletPrefab"/> above the player.
    /// </summary>
    /// <param name="ctx">Input callback context provided by the Input System.</param>
    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + fireRate;
        GameManager.Instance.PlayShoot();
        Instantiate(bulletPrefab,
            transform.position + Vector3.up * 0.6f,
            Quaternion.identity);
    }
}
