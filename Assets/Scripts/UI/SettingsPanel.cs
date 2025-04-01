using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPanel : MonoBehaviour
{
    public void OnRestart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}