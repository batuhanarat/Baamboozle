


using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPopUpWithTeamOptions : SpecialPopUp
{
    private List<Button> buttons;

    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject buttonPrefab;

    void Start()
    {
        var otherTeams = ServiceProvider.TeamManager.GetOtherTeamsExceptCurrent();
        buttons = new List<Button>(otherTeams.Count);
        foreach (var team in otherTeams)
        {
            GameObject button = Instantiate(buttonPrefab,buttonContainer);
            Button button1 = button.GetComponent<Button>();
            button.GetComponentInChildren<TextMeshProUGUI>().text = team.teamName;
            buttons.Add(button1);
            button1.onClick.AddListener(() => OnButtonPressed(team.teamID));
        }

    }

    public void OnButtonPressed(int teamID)
    {
        var card1 = (SpecialCardWithOptions) specialCard;
        card1.Execute(teamID.ToString());
        card.DeactivateCard();
        ServiceProvider.ScoreManager.DecreaseCard();

        Close();
    }

}