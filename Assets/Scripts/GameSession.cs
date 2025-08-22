using UnityEngine;

public class GameSession : MonoBehaviour, IPlayerSessionData, INameInputWriter
{
    public static GameSession Instance { get; private set; }
    public string PlayerNick { get; private set; }

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

    public void SetName(string name)
    {
        PlayerNick = name;  
    }
}
