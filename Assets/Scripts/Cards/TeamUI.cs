using UnityEngine;

public class TeamUI : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI teamNameText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    public int teamID;
    public string teamName;
    public int teamScore;
    private Color originalColor;

    public void SetTeamNumber(int id)
    {
        var textForPlayer = id +1  ;
        teamNameText.text = textForPlayer.ToString() +". Takım";
        if(id == 0) SetCurrentTeam();
    }

    public void SetCurrentTeam()
    {
        teamNameText.color = Color.red;
        scoreText.color =  Color.red;
    }
    public void SetOtherTeam()
    {
        teamNameText.color  = originalColor;
        scoreText.color =  originalColor;
    }

    public void ChangeName(string name)
    {
        teamNameText.text = name;

    }

    public void Initialize(int teamID, string teamName, int teamScore)
    {
        this.teamID = teamID;
        this.teamName = teamName;
        this.teamScore = teamScore;
        originalColor = teamNameText.color;
    }

    public void UpdateScore(int scoreToBeAdded)
    {
        teamScore += scoreToBeAdded;
        scoreText.text = teamScore.ToString();
    }
    public void ClearScore()
    {
        teamScore = 0;
        scoreText.text = teamScore.ToString();
    }

}
