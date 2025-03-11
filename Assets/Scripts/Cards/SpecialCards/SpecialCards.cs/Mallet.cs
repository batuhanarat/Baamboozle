using UnityEngine;

public class Mallet : SpecialCardBase
{
    private static int MALLET_SCORE = 5;
    public override void Execute()
    {
        TeamManager teamManager =  ServiceProvider.TeamManager;
        TeamUI lowestTeam = teamManager.GetLowestTeam();
        lowestTeam.UpdateScore(-MALLET_SCORE);
        ServiceProvider.TeamManager.ChangeTeam();

    }
}