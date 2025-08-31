using Unity.Netcode;

public class ScoreboardService : NetworkBehaviour
{
    public static ScoreboardService Instance { get; private set; }

    public NetworkList<PlayerStats> PlayerStatsList = new(readPerm: NetworkVariableReadPermission.Everyone, writePerm: NetworkVariableWritePermission.Server);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPlayer(ulong playerId, string playerName)
    {
        foreach (var player in PlayerStatsList)
        {
            if (player.clientID == playerId) return;
        }

        var stats = new PlayerStats
        {
            Kills = 0,
            Deaths = 0,
            clientID = playerId,
            displayName = playerName
        };
        PlayerStatsList.Add(stats);
    }

    public void OnPlayerKilled(ulong kilerId, ulong victimId)
    {
        for (int i = 0; i < PlayerStatsList.Count; i++)
        {
            var stats = PlayerStatsList[i];
            if (PlayerStatsList[i].clientID == kilerId)
            {
                stats.Kills += 1;
                PlayerStatsList[i] = stats;
            }
            else if (PlayerStatsList[i].clientID == victimId)
            {
                stats.Deaths += 1;
                PlayerStatsList[i] = stats;
            }
        }
    }


}
