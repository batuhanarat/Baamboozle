


using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPopUpWithPointOptions : SpecialPopUp
{
    [SerializeField] List<Button> buttons;

    void Start()
    {
        foreach (var button in buttons)
        {
            var pointAsString = button.GetComponentInChildren<TextMeshProUGUI>().text;
            button.onClick.AddListener(() => OnButtonPressed(pointAsString));
        }
    }

    public void OnButtonPressed(string point)
    {
        var cardWithOptions = specialCard as SpecialCardWithOptions;
        cardWithOptions.Execute(point);
        card.DeactivateCard();
        Close();
    }

    public override void Init()
    {
        base.Init();
        ChangeDescription();
    }

    public void ChangeDescription()
    {
        var text = GetDescription().text;
        var card = GetComponentInChildren<SpecialCardBase>();
        var cardWithOptions = (SpecialCardWithOptions) card;

        var chosenTeam = cardWithOptions.GetChosenTeamName();
        var newText = chosenTeam +text;
        SetDescription(newText);
    }

}