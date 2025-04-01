using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI winningTeam;



    void Start()
    {
        SetWinner();
    }


    public void SetWinner()
    {
        var team = ServiceProvider.TeamManager.GetTopTeam();
        winningTeam.text = team.teamName + " kazandı! ";
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

}