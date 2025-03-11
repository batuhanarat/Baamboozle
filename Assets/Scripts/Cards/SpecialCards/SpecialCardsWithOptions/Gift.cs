public class Gift : SpecialCardWithOptions
{

    void Awake()
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        teamManager.GetRandomTeam();
    }

    public override string GetChosenTeamName()
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        var randomlyChosenTeam = teamManager.RandomlyChosenTeam;
        return randomlyChosenTeam.teamName;
    }

    public override void Execute(string option)
    {
        TeamManager teamManager = ServiceProvider.TeamManager;
        var chosenPoint = int.Parse(option);

        var activeTeam = teamManager.GetActiveTeam();
        activeTeam.UpdateScore(-chosenPoint);

        var randomlyChosenTeam =  teamManager.RandomlyChosenTeam;
        randomlyChosenTeam.UpdateScore(chosenPoint);
        ServiceProvider.TeamManager.ChangeTeam();


    }
}