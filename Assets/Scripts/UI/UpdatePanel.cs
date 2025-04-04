using TMPro;
using UnityEngine;

public class UpdatePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorText;

    private const string LoggedKey = "logged_in";

    public void OnButtonPressed()
    {
        var code =  inputField.text;
        errorText.color = Color.yellow;
        errorText.text = "Yükleniyor";
        ServiceProvider.QuestionManager.UpdateQuestions(code);
    }

    public void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Error: " + message);
        }
    }
    public void ShowSuccess(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
            errorText.color = Color.green; // Assuming you want to visually indicate success
        }
        else
        {
            Debug.Log("Success: " + message);
        }
    }


}