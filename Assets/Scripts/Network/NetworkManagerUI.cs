using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button _hostButton;
    [SerializeField] private Button _clientButton;
    public TMP_InputField _inputField;

    private void Awake()
    {
        _inputField.onEndEdit.AddListener(SubmitName);

        _hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
        _clientButton.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
    }

    void SubmitName(string name)
    {
        if (name != "")
        {
            GameSession.Instance.PlayerNick = name;
            _hostButton.gameObject.SetActive(true);
            _clientButton.gameObject.SetActive(true);
            _inputField.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Invalid name!");
        }
    }

    //private void StartHost()
    //{



    //    if (_inputField.text.ToString() == "cihan")
    //    {
    //        NetworkManager.Singleton.StartHost();
    //    }

    //}
}
