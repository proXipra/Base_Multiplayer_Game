using UnityEngine;

public interface INameValidator
{
    bool IsValid(string name);
    string ErrorMessage {  get; }
}
