using System.Collections.Generic;
using Ricimi;
using UnityEngine;
using UnityEngine.UI;

public class TeamNameUIController : MonoBehaviour
{
    public Button selectButton;
    public GameObject TeamNameUI;
    public Transform holder;
    private List<TeamNameUI> teamNameUIs = new();
    private string[] teamNames;
    int count;


    void Start()
    {
        count = GameSettings.GetTeamSize();
        for(int i = 0 ; i< count ; i++)
        {
            var teamuı = Instantiate(TeamNameUI,holder);
            teamNameUIs.Add(teamuı.GetComponent<TeamNameUI>());
        }
    }


    public void OnSelectButtonClicked()
    {

        teamNames = new string[count];

        for(int i = 0 ; i< count ; i++)
        {
            Debug.Log("select " + teamNameUIs[i].teamName);
            teamNames[i] = teamNameUIs[i].teamName;
        }

        GameSettings.TeamNames = teamNames;
        GameSettings.isTeamNamesChangedFromSettings = true;
        Debug.Log("team names count " + GameSettings.GetTeamNames().Length);
        GetComponent<SceneTransition>().PerformTransition();
    }

}