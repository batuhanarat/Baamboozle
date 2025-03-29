using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSizeSelector : MonoBehaviour, IProvidable
{
    public ToggleGroup toggleGroup;
    private int _cardSize;

    private Dictionary<int , int[]> cardSizeMapper = new()
    {
        {1, new int[]{  12,16,18,20,24,48 } },
        {2, new int[]{  12,16,18,20,24,48 }},
        {3, new int[]{  12,15,18,24,30,48 }},
        {4, new int[]{  12,16,20,24,32,48 }},
        {5, new int[]{  15,20,25,30,35,45 }},
        {6, new int[]{  12,18,24,30,36,42 }},
        {7, new int[]{  7,14,21,28,35,42 }},
        {8, new int[]{  8,16,24,32,40,48 }}
    };



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

    public void ChangeCardSize(int teamSize)
    {
        GameSettings.TotalQuestionCount = 100;
        var arraycard = cardSizeMapper[teamSize];
        List<TextMeshProUGUI> toogleList = new();

        foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            var textmesh =  toggle.GetComponentInChildren<TextMeshProUGUI>();
            toogleList.Add(textmesh);
        }

        for(int i = 0 ; i< arraycard.Length ; i++)
        {
            if(arraycard[i] < GameSettings.GetTotalQuestionCount())
            {
                toogleList[i].text = arraycard[i].ToString();
            } else
            {
                toogleList[i].GetComponentInParent<Toggle>().gameObject.SetActive(false);
            }
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