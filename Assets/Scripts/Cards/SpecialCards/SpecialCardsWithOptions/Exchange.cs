

public class Exchange : SpecialCardWithOptions
{

    public override void Execute(string option)
    {
        int id = int.Parse(option);
        TeamManager teamManager = ServiceProvider.TeamManager;
        var activeTeam = teamManager.GetActiveTeam();
        var chosenTeam = teamManager.GetTeamFromID(id);

        var ourScore  = activeTeam.teamScore;
        var theirScore = chosenTeam.teamScore;

        activeTeam.ClearScore();
        chosenTeam.ClearScore();

        activeTeam.UpdateScore(theirScore);
        chosenTeam.UpdateScore(ourScore);
        ServiceProvider.TeamManager.ChangeTeam();
    }

    public override string GetChosenTeamName()
    {
        return "";
    }
}