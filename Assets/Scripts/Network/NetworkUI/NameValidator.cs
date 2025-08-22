using UnityEngine;

public class NameValidator : INameValidator
{
    public string ErrorMessage => "Name is not valid!";

    public bool IsValid(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}
