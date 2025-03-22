
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : IProvidable
{

    public TeamManager()
    {
        ServiceProvider.Register(this);
    }

    private int teamCount;
    private List<TeamUI> teams;

    private int teamIndex = 0;
    public TeamUI RandomlyChosenTeam;

    public void CreateTeams(int count)
    {
        teamCount= count;
        teams = new List<TeamUI>(count);
    }

    public void AddTeam(TeamUI team, int id)
    {
        var teamIdForPlayer = id + 1;
        team.Initialize(id, teamIdForPlayer.ToString() +".Takım " , 0);
        teams.Add(team);
        team.SetTeamNumber(id);
    }

    public TeamUI GetTeamFromID(int id)
    {
        foreach(TeamUI team in teams)
        {
            if(team.teamID == id)
            {
                return team;
            }
        }
        return null;
    }

    public TeamUI GetTeamFromName(string name)
    {
        foreach(TeamUI team in teams)
        {
            if(team.teamName == name)
            {
                return team;
            }
        }
        return null;
    }

    public TeamUI GetActiveTeam()
    {
        return teams[teamIndex];
    }
    public void ChangeTeam()
    {
        GetActiveTeam().SetOtherTeam();
        teamIndex++;
        teamIndex %= teamCount;
        GetActiveTeam().SetCurrentTeam();
    }

    public TeamUI GetTopTeam()
    {
        TeamUI topTeam = teams[0];
        foreach(TeamUI team in teams)
        {
            if(team.teamScore > topTeam.teamScore)
            {
                topTeam = team;
            }
        }
        return topTeam;
    }

    public TeamUI GetLowestTeam()
    {
        TeamUI lowestTeam = teams[0];
        foreach(TeamUI team in teams)
        {
            if(team.teamScore < lowestTeam.teamScore)
            {
                lowestTeam = team;
            }
        }
        return lowestTeam;
    }

    public TeamUI GetRandomTeam()
    {
        var otherTeams =  GetOtherTeamsExpectCurrent(teamIndex);
        int randomIndex = Random.Range(0, otherTeams.Count);
        RandomlyChosenTeam = otherTeams[randomIndex];
        return RandomlyChosenTeam;
    }

    public List<TeamUI> GetAllTeams()
    {
        return teams;
    }

    public List<TeamUI> GetOtherTeamsExceptCurrent()
    {
        return GetOtherTeamsExpectCurrent(teamIndex);
    }

    private List<TeamUI> GetOtherTeamsExpectCurrent(int teamID)
    {
        List<TeamUI> otherTeams = new();
        foreach(TeamUI team in teams)
        {
            if(team.teamID != teamID)
            {
                otherTeams.Add(team);
            }
        }
        return otherTeams;
    }

    public TeamUI GetNextTeam()
    {

        if(teamIndex == teamCount - 1)
        {
            return teams[0];
        }
        return teams[teamIndex + 1];
    }

}

