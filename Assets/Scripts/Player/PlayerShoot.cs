using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.5f;

    private InputSystem_Actions _input;
    private float _nextFireTime;

    void Awake()
    {
        _input = new InputSystem_Actions();
    }

    void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Attack.performed += OnFire;
    }

    void OnDisable()
    {
        _input.Player.Attack.performed -= OnFire;
        _input.Player.Disable();
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + fireRate;
        Instantiate(bulletPrefab,
            transform.position + Vector3.up * 0.6f,
            Quaternion.identity);
    }
}
