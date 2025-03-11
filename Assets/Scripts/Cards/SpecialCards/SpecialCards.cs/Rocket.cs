public class Rocket : SpecialCardBase
{
    private static int ROCKET_THRESHOLD = 60;
    private static int ROCKET_SCORE = 5;
    //Roket (O an birinci olan grubun puanı 60 ise +5 puan fazlası olacak çıkarsa) +
    public override void Execute()
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        var topTeam = teamManager.GetTopTeam();
        var score = topTeam.teamScore;
        if(score >= ROCKET_THRESHOLD)
        {
            teamManager.GetActiveTeam().UpdateScore(score + ROCKET_SCORE);
        }
        ServiceProvider.TeamManager.ChangeTeam();


    }
}