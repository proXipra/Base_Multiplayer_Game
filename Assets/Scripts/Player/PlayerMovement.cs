using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    private PlayerInputActions _playerInput;
    [SerializeField] private float _speed;
    private Camera _playerCamera;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            _playerCamera = Camera.main;
            _playerCamera.transform.SetParent(transform, true);
        }
    }

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    

    private void OnEnable()
    {
        _playerInput.Enable();

    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
        Vector2 lookInput = _playerInput.Player.Look.ReadValue<Vector2>();

        //Debug.Log($"Move input X:{moveInput.x}, Y:{moveInput.y}");
        //Debug.Log($"Look input X:{lookInput.x}, Y:{lookInput.y}");

        transform.Translate(new Vector3 (moveInput.x, 0,moveInput.y) * _speed * Time.deltaTime);
        transform.rotation *= Quaternion.Euler(0, lookInput.x, 0);
    }
}
