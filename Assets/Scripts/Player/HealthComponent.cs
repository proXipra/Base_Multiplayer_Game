using Unity.Netcode;
using UnityEngine;

public class HealthComponent : NetworkBehaviour
{
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
        ScoreboardService.Instance?.UptadeStats(killerId, victimId);
        //RespawnCoroutine yazýlacak !!
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
