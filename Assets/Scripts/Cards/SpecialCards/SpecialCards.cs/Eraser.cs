public class Eraser : SpecialCardBase
{
    public override void Execute()
    {
        TeamManager teamManager =  ServiceProvider.TeamManager;
        teamManager.GetActiveTeam().ClearScore();
        ServiceProvider.TeamManager.ChangeTeam();

    }
}