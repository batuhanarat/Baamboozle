using System.Collections.Generic;
using UnityEngine;

public class Virus : SpecialCardBase
{
    public override void Execute()
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        List<TeamUI> teams = teamManager.GetAllTeams();
        foreach (var team in teams)
        {
            team.ClearScore();
        }
        ServiceProvider.TeamManager.ChangeTeam();

    }
}