using System.Collections.Generic;
using UnityEngine;

public class TeamPanel : MonoBehaviour
{
    private int TeamCount = 4;
    [SerializeField] private GameObject teamPrefab;

    private List<TeamUI> teams = new();

    void Start()
    {
        TeamCount = GameSettings.GetTeamSize();


        ServiceProvider.TeamManager.CreateTeams(TeamCount);
        for(int i = 1 ; i<= TeamCount ; i++)
        {
            GameObject team = Instantiate(teamPrefab, transform);
            TeamUI teamUI = team.GetComponent<TeamUI>();
            teams.Add(teamUI);
            ServiceProvider.TeamManager.AddTeam(teamUI, i-1);
        }

        ServiceProvider.TeamManager.GetActiveTeam().SetCurrentTeam();

        if(GameSettings.GetTeamNames() == null)
        {
            return;
        }
        else
        {
            var names = GameSettings.GetTeamNames();
            for(int i = 0 ; i< TeamCount ; i++)
            {
                string name = names[i];
                teams[i].ChangeName(name);
            }
        }
    }

}
