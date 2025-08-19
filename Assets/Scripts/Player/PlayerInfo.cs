using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> playerName = new("Unnamed", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] private TMPro.TextMeshPro _nameText;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            playerName.Value = new FixedString128Bytes(GameSession.Instance.PlayerNick);
        }

        _nameText.text = playerName.Value.ToString();

        
    }

    private void OnEnable()
    {
        playerName.OnValueChanged += OnNameChanged;
    }

    private void OnDisable()
    {
        playerName.OnValueChanged -= OnNameChanged;
    }

    private void OnNameChanged(FixedString128Bytes previousValue, FixedString128Bytes newValue)
    {
        _nameText.text = newValue.ToString();
    }
}
