using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamSizeSelector : MonoBehaviour , IProvidable
{
    public ToggleGroup toggleGroup;
    private int _teamSize;
    private bool _changedFromSettings = false;
    public int SelectedTeamSize
    {
        get
        {
            return _changedFromSettings ? _teamSize : 2;
        }
    }

    void Awake()
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

            if (int.TryParse(toggleLabel, out int teamSize))
            {
                _teamSize = teamSize;
                GameSettings.TeamSize = _teamSize;
                GameSettings.isTeamSizeChangedFromSettings = true;
            }
            else
            {
                Debug.LogError("Toggle metni sayıya çevrilemedi: " + toggleLabel);
            }
        }
    }
}