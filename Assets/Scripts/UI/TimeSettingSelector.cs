using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeSettingSelector : MonoBehaviour , IProvidable
{
    public ToggleGroup toggleGroup;
    private TimeSetting _timeSetting;
    private bool _changedFromSettings = false;
    public TimeSetting OptionCountForMultipleChoiceQuestionsSize
    {
        get
        {
            return _changedFromSettings ? _timeSetting : TimeSetting.DECREASING;
        }
    }

    public void Awake()
    {
        ServiceProvider.Register(this);
    }

    void Start()
    {
        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((isOn) => OnToggleValueChanged(toggle, isOn));
        }
    }

    void OnToggleValueChanged(Toggle changedToggle, bool isOn)
    {
        if (isOn)
        {
            string toggleLabel = changedToggle.GetComponentInChildren<TextMeshProUGUI>().text;
            if (toggleLabel == "Her soru icin 60 sn azalan puan")
            {
                _timeSetting = TimeSetting.DECREASING;
                GameSettings.isTimeSettingsChangedFromSettings = true;
                GameSettings.TimeSetting = _timeSetting;
                Debug.Log("Azalan puan modu seçildi.");
            }
            else if (toggleLabel == "Her soru icin 60 sn standart puan")
            {
                _timeSetting = TimeSetting.STANDART;
                    GameSettings.isTimeSettingsChangedFromSettings = true;
                GameSettings.TimeSetting = _timeSetting;
                Debug.Log("Standart puan modu seçildi.");
            }
            else if (toggleLabel == "Suresiz")
            {
                _timeSetting = TimeSetting.NO_TIME;
                GameSettings.isTimeSettingsChangedFromSettings = true;
                GameSettings.TimeSetting = _timeSetting;
                Debug.Log("Süresiz mod seçildi.");
            }
            else
            {
                Debug.LogWarning("Geçersiz toggleLabel değeri: " + toggleLabel);
                _changedFromSettings = false;
            }
        }
    }
}

public enum TimeSetting
{
    NO_TIME,
    STANDART,
    DECREASING
}