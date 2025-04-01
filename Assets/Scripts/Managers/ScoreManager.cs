using UnityEngine;

public class ScoreManager :  MonoBehaviour, IProvidable
{

    [SerializeField] public GameObject WinPanel;
    public void Awake()
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
        Instantiate(WinPanel,ServiceProvider.AssetLibrary.GamePopupRoot);
    }

}