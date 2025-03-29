using UnityEngine;
using TMPro;

public class TeamNameUI : MonoBehaviour
{
    [Header("References")]
    public TMP_InputField nameInputField;


    [Header("Validation Settings")]
    public int minNameLength = 3;
    public int maxNameLength = 20;
    public string teamName;

    private void Start()
    {
        nameInputField.onValueChanged.AddListener(SetName);
    }

    public void SetName(string name)
    {
        teamName = name;
    }

    public void OnNameWritten()
    {
        string inputName = nameInputField.text;

        if (IsNameValid(inputName))
        {
            SetName(inputName);
        }
    }


    private bool IsNameValid(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return false;
        }

        // Check name length
        if (name.Length < minNameLength || name.Length > maxNameLength)
        {
            return false;
        }

        return true;
    }

}