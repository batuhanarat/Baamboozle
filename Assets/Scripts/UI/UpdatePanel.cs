using TMPro;
using UnityEngine;

public class UpdatePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorText;

    private const string LoggedKey = "logged_in";

    public async void OnButtonPressed()
    {
        var code =  inputField.text;
        errorText.color = Color.yellow;
        errorText.text = "Yükleniyor";
        ServiceProvider.QuestionManager.DeletePrefsAndLoadQuestions(LoggedSuccessfull,HandleError);
    }

    public void HandleError()
    {
        errorText.color = Color.red;
        errorText.text = "Baglantı Sağlanamadı";
    }

    public void LoggedSuccessfull()
    {
        PlayerPrefs.SetInt(LoggedKey,1);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }

}