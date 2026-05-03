using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float screenHalfWidth = 8.5f;

    private InputSystem_Actions _input;

    void Awake()
    {
        _input = new InputSystem_Actions();
    }

    void OnEnable()
    {
        _input.Player.Enable();
    }

    void OnDisable()
    {
        _input.Player.Disable();
    }

    void Update()
    {
        float moveX = _input.Player.Move.ReadValue<Vector2>().x;

        Vector3 pos = transform.position;
        pos.x += moveX * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -screenHalfWidth, screenHalfWidth);
        transform.position = pos;
    }
}
