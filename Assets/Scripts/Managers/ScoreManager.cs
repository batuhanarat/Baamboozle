using UnityEngine;

public class ScoreManager : IProvidable
{

    public GameObject WinPanel;
    public ScoreManager()
    {
        ServiceProvider.Register(this);
    }

    public int TotalCardsToBeAnswered;

    public void SetData(int cards)
    {
        Debug.Log("data set");

        TotalCardsToBeAnswered = cards;
    }

    public void DecreaseCard()
    {
        TotalCardsToBeAnswered--;
        Debug.Log("TotalCardsToBeAnswered" + TotalCardsToBeAnswered);
        if(TotalCardsToBeAnswered == 0)
        {
            GameFinished();
        }
    }

    private void GameFinished()
    {
        Debug.Log("game finished");
        TeamUI winnerTeam = ServiceProvider.TeamManager.GetTopTeam();
        Debug.Log("winner " + winnerTeam.teamName);


       // WinPanel.gameObject.SetActive(true);
    }

}