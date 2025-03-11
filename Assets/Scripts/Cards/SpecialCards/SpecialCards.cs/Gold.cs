using UnityEngine;

public class Gold : SpecialCardBase
{
    private static int GOLD_SCORE = 50;
    public override void Execute()
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        var activeTeam = teamManager.GetActiveTeam();
        activeTeam.UpdateScore(GOLD_SCORE);
        ServiceProvider.TeamManager.ChangeTeam();

    }
}