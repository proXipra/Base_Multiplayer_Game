using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class HealthComponent : NetworkBehaviour
{
    [SerializeField] private float _maxHealth = 100;
    public NetworkVariable<float> health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public void ApplyDamage(float damage, ulong attackerId)
    {

        health.Value -= damage;
        LogDamageClientRpc(damage);

        if (health.Value <= 0)
        {
            Die(attackerId, OwnerClientId);
            OnDeathClientRpc(attackerId, OwnerClientId);
            return;
        }
    }

    void Die(ulong killerId, ulong victimId)
    {
        ScoreboardService.Instance.OnPlayerKilled(killerId, victimId);
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        SetAliveClientRpc(false);
        yield return  new WaitForSeconds(5f);
        health.Value = 100;
        SetAliveClientRpc(true);
    }

    [ClientRpc]
    void SetAliveClientRpc(bool alive)
    {
        Renderer[] renderers =  GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) 
        {
            renderer.enabled = alive;
        }
        Debug.Log($"Renders disabled {OwnerClientId}");

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = alive;
        }
        Debug.Log($"Colliders disabled {OwnerClientId}");
        PlayerMovement controller = GetComponent<PlayerMovement>();
        if (controller) controller.enabled = alive;

        WeaponController weaponController = GetComponent<WeaponController>();
        if (weaponController) weaponController.enabled = alive;
    }


    [ClientRpc]
    void LogDamageClientRpc(float damage)
    {
        Debug.Log($"Player: {OwnerClientId} took, {damage} damage  .");
    }

    

    [ClientRpc]
    void OnDeathClientRpc(ulong attackerId, ulong ownerId)
    {
        Debug.Log($"Player: {ownerId} is killed by player {attackerId} .");
    }


}
