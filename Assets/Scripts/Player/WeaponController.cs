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

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + RAY_OFFSET, transform.position.z), forward, Color.red, 20f);

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + RAY_OFFSET, transform.position.z), 
            transform.forward, out hitInfo, 10f, hitMask))
        {
            HitClientRpc();
        }
    }


    [ClientRpc]
    private void HitClientRpc()
    {
        Debug.LogWarning("Hit occured |ClientRPC|");
    }
}
