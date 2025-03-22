using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionCountSelectorForMultipleChoiceQuestion : MonoBehaviour, IProvidable
{
    public ToggleGroup toggleGroup;
    private int _optionCountForMultipleChoiceQuestionsSize;
    private bool _changedFromSettings = false;
    public int OptionCountForMultipleChoiceQuestionsSize
    {
        get
        {
            return _changedFromSettings ? _optionCountForMultipleChoiceQuestionsSize : 4;
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

            if (int.TryParse(toggleLabel, out int optionCountForMultipleChoiceQuestionsSize))
            {
                _optionCountForMultipleChoiceQuestionsSize = optionCountForMultipleChoiceQuestionsSize;
                GameSettings.isOptionCountsChangedFromSettings = true;
                GameSettings.OptionCounts = _optionCountForMultipleChoiceQuestionsSize;
            }
            else
            {
                Debug.LogError("Toggle metni sayıya çevrilemedi: " + toggleLabel);
            }
        }
    }
}