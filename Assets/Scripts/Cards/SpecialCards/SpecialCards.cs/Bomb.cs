public class Bomb : SpecialCardBase
{
    private static int BOMB_SCORE = 50;

    public override void Execute()
    {
        TeamUI team  = ServiceProvider.TeamManager.GetActiveTeam();
        team.UpdateScore(-BOMB_SCORE);
        ServiceProvider.TeamManager.ChangeTeam();
    }
}