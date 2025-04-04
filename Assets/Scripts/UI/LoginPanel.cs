using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private AdPanelController adPanelController;


    public  void OnButtonPressed()
    {
        var code =  inputField.text;
        errorText.color = Color.yellow;
        errorText.text = "Yükleniyor";
        ServiceProvider.QuestionManager.OnLoginButtonClicked(code);
        //await ApiManager.Instance.GetQuestions(code,"baamboozle", LoggedSuccessfull, HandleError);
    }

/*
    public async void OnButtonPressedOfflineTest()
    {
        var code =  inputField.text;
        errorText.color = Color.yellow;
        errorText.text = "Yükleniyor";
        ServiceProvider.QuestionManager.LoadQuestionsIfNeeded();
        LoggedSuccessfull();
        AddsSuccessfullyGet();
        adPanelController.TryShowAdd();
    }
*/
    public void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Error: " + message);
        }
    }



}