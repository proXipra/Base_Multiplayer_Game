using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkControlUI : MonoBehaviour, INetworkControlUI
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private NameInputUI _nameInput;
    

    private void Awake()
    {
        _hostButton.onClick.AddListener(StartHost);
        _clientButton.onClick.AddListener(StartClient);

        if (_nameInput != null)
        {
            _nameInput.OnNameSubmitted += OnNameEntered;
        }
    }

    public void StartHost()
    {
        ToggleButtons(false);
        NetworkManager.Singleton.StartHost();
    }
    public void StartClient()
    {
        ToggleButtons(false);
        NetworkManager.Singleton.StartClient();
    }

    void OnNameEntered(string name)
    {
        ToggleButtons(true);
    }

    private void ToggleButtons(bool state)
    {
        _hostButton.gameObject.SetActive(state);
        _clientButton.gameObject.SetActive(state);
    }


}
