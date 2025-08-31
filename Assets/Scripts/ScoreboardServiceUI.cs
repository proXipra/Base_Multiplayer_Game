using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreboardServiceUI : NetworkBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;



    public override void OnNetworkSpawn()
    {
        ScoreboardService.Instance.PlayerStatsList.OnListChanged += OnListChangeUI;
        UpdateUI();
    }

    public override void OnNetworkDespawn()
    {
        ScoreboardService.Instance.PlayerStatsList.OnListChanged -= OnListChangeUI;
    }

    private void OnListChangeUI(NetworkListEvent<PlayerStats> changeEvent)
    {
        UpdateUI();
    }


    
    private void UpdateUI()
    {
        Debug.Log("UI update initiated");
        if (ScoreboardService.Instance.PlayerStatsList == null) return;
        Debug.Log("List update initiated");
        List<PlayerStats> temp = new();

        foreach (var s in ScoreboardService.Instance.PlayerStatsList)
        {
            temp.Add(s);
        }

        var ordered = temp.OrderByDescending(s => s.Kills).ThenBy(s => s.Deaths).ToList();
        string boardText = "Scoreboard\n";

        for (int i = 0; i < ordered.Count; i++)
        {
            boardText += $"{ordered[i].displayName} Kills:{ordered[i].Kills} Deaths:{ordered[i].Deaths}\n";
        }
        _text.text = boardText;
    }
}
