using TMPro;
using UnityEngine;
using static NameInputUI;

public class NameInputUI : MonoBehaviour, INameInputUI
{
    public delegate void NameSubmitted(string playerName);
    public event NameSubmitted OnNameSubmitted;

    [SerializeField] private TMP_InputField _inputField; 
    private INameValidator _nameValidator;

    private void Awake()
    {
        _nameValidator = new NameValidator();

        _inputField.onEndEdit.AddListener(SubmitName);
    }

    public void SubmitName(string name)
    {
        if (!_nameValidator.IsValid(name))
        {
            Debug.LogError(_nameValidator.ErrorMessage);
            return;
        }
        GameSession.Instance.SetName(name);
        OnNameSubmitted?.Invoke(name);
        _inputField.gameObject.SetActive(false);
    }
}
