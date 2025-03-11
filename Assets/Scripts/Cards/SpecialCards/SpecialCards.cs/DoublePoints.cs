using System;
using UnityEngine;

public class DoublePoints : SpecialCardBase
{
    public override void Execute()
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        var activeTeam = teamManager.GetActiveTeam();
        activeTeam.UpdateScore(Math.Abs(activeTeam.teamScore) * 2);
        ServiceProvider.TeamManager.ChangeTeam();

    }
}