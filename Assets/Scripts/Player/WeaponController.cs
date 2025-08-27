using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : NetworkBehaviour
{
    private PlayerInputActions _playerInput;
    private InputAction _attackAction;

    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float _weaponDamage = 25f;
    [SerializeField] private float _weaponRange = 10f;
    private const float RAY_OFFSET = 1.5f;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _attackAction = _playerInput.Player.Attack;
    }

    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        
        _playerInput.Enable();
        _attackAction.performed += Fire;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _playerInput.Disable();
        _attackAction.performed -= Fire;
    }
    void Fire(InputAction.CallbackContext callbackContext)
    {

        Vector3 origin = new Vector3(transform.position.x, transform.position.y + RAY_OFFSET, transform.position.z);
        Vector3 direction = transform.forward;
        FireServerRpc(origin, direction);
    }

    [ServerRpc]
    void FireServerRpc(Vector3 origin, Vector3 dir)
    {
        Debug.DrawRay(origin, dir * 10, Color.red, 20f);

        if (Physics.Raycast(origin, dir, out RaycastHit hitInfo, _weaponRange, hitMask))
        {
            if (hitInfo.collider.GetComponentInParent<HealthComponent>() is HealthComponent healthComp)
            {
                healthComp.ApplyDamage(_weaponDamage, OwnerClientId);
            }

        }
    }

}
