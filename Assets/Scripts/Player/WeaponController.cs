using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WeaponController : NetworkBehaviour
{
    private PlayerInputActions _playerInput;
    private InputAction _attackAction;

    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float _weaponDamage = 25f;
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
        _attackAction.performed += AttackPerformed;
    }


    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        _playerInput.Disable();
        _attackAction.performed -= AttackPerformed;
    }




    void AttackPerformed(InputAction.CallbackContext callbackContext)
    {
        AttackServerRpc();
    }

    [ServerRpc]
    void AttackServerRpc()
    {
        RaycastHit hitInfo;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + RAY_OFFSET, transform.position.z);
        //Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        Debug.DrawRay(origin, transform.forward * 10, Color.red, 20f);

        if (Physics.Raycast(origin, transform.forward, out hitInfo, 10f, hitMask))
        {
            if (hitInfo.collider.GetComponentInParent<HealthComponent>() is HealthComponent healthComp)
            {
                healthComp.ApplyDamageServerRpc(_weaponDamage, OwnerClientId);
            }
        
            else
            {
                LogClientRpc();
            }
        }
    }

    [ClientRpc]
    void LogClientRpc()
    {
        Debug.Log("Hit happend but lack of component");
    }

    //[ClientRpc]
    //private void HitClientRpc(Transform transform)
    //{
    //    Debug.LogWarning("Hit");

    //    transform.GetComponent<HealthComponent>().ApplyDamageServerRpc(_weaponDamage, OwnerClientId);
    //}
}
