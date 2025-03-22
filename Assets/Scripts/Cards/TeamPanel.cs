using UnityEngine;

public class TeamPanel : MonoBehaviour
{
    private int TeamCount = 4;
    [SerializeField] private GameObject teamPrefab;

    void Start()
    {
        TeamCount = GameSettings.GetTeamSize();


        ServiceProvider.TeamManager.CreateTeams(TeamCount);
        for(int i = 1 ; i<= TeamCount ; i++)
        {
            GameObject team = Instantiate(teamPrefab, transform);
            TeamUI teamUI = team.GetComponent<TeamUI>();

            ServiceProvider.TeamManager.AddTeam(teamUI, i-1);
        }
        ServiceProvider.TeamManager.GetActiveTeam().SetCurrentTeam();
    }

}
