using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    private const string LoggedKey = "logged_in";
    private const string CanShowAddKey = "can_show_add";


    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private AdPanelController adPanelController;

    void Awake()
    {
        if(PlayerPrefs.HasKey(LoggedKey))
        {
           gameObject.SetActive(false);
        }
    }

    public async void OnButtonPressed()
    {
        var code =  inputField.text;
        errorText.color = Color.yellow;
        errorText.text = "Yükleniyor";
        await ApiManager.Instance.GetQuestions(code,"baamboozle", LoggedSuccessfull, HandleError);
        await ApiManager.Instance.GetAds(code, AddsSuccessfullyGet, ErrorOnLoadingAdds);
    }
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

    public void AddsSuccessfullyGet()
    {
        PlayerPrefs.SetInt(CanShowAddKey,1);
        PlayerPrefs.Save();
    }

    public void ErrorOnLoadingAdds()
    {
        PlayerPrefs.SetInt(CanShowAddKey,0);
        PlayerPrefs.Save();
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