using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using Unity.Collections;

public class ScoreboardService : NetworkBehaviour
{
    public static ScoreboardService Instance { get; private set; }

    public NetworkList<PlayerStats> PlayerStats = new(null, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //PlayerStats = new NetworkList<PlayerStats>(Allocator.Persistent);
    }

    public void UptadeStats(ulong kilerId, ulong victimId)
    {
        for (int i = 0; i < PlayerStats.Count; i++)
        {
            var stats = PlayerStats[i];
            if (PlayerStats[i].clientID == kilerId)
            {
                stats.Kills += 1;
                PlayerStats[i] = stats;
            }
            else if (PlayerStats[i].clientID == victimId)
            {
                stats.Deaths += 1;
                PlayerStats[i] = stats;
            }
        }
    }


}
