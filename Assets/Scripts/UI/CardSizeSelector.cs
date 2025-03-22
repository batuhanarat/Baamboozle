using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSizeSelector : MonoBehaviour, IProvidable
{
    public ToggleGroup toggleGroup;
    private int _cardSize;

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

            if (int.TryParse(toggleLabel, out int cardSize))
            {
                _cardSize = cardSize;
                GameSettings.CardSize = _cardSize;
                GameSettings.isCardSizeChangedFromSettings = true;
            }
            else
            {
                Debug.LogError("Toggle metni sayıya çevrilemedi: " + toggleLabel);
            }
        }
    }
}