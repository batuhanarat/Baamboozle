using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockText;
    [SerializeField] private TextMeshProUGUI pointText;
    private float timeRemaining = 30f; // 30 saniye
    private bool timerIsRunning = false;

    public void StartTimer()
    {
        timerIsRunning = true;

        if (clockText == null)
        {
            clockText = GetComponent<TextMeshProUGUI>();
        }

        UpdateTimerDisplay();
    }



    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();

            }
        }
    }

    public void SetTimerValue(int startTimeRemaining)
    {
        timeRemaining = startTimeRemaining;
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        var remainingTimeString = string.Format("{0}:{1:00}", minutes, seconds);
        clockText.text = remainingTimeString;
        var pointString = string.Format("{0}", seconds);

        pointText.text = pointString + " puan";

    }

    public void ResetTimer()
    {
        timeRemaining = 30f;
        timerIsRunning = true;
    }
}