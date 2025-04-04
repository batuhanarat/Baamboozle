using TMPro;
using UnityEngine;

public class AdPanelController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public GameObject CloseButton;
    [SerializeField] public GameObject adCanvas;
    [SerializeField] public VidPlayer videoPlayer;
    [SerializeField] public ImageAdShower imageAdShower;

    [SerializeField] public GameObject RawImage;

    private const string CanShowAddKey = "can_show_add";


    private bool TimerActivated = false;
    private float videoDuration = 10f;
    private float imageDuration = 10f;
    public AdType currentAdType =  AdType.Image;
    private float timeRemaining;

    public void Start()
    {
       // TryShowAdd();
    }

    public void TryShowAdd()
    {
        if(!PlayerPrefs.HasKey(CanShowAddKey)) return;

        ShowAd(currentAdType);
    }

    void Update()
    {

        if(!TimerActivated) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            int timeRemainingINT =  (int)(timeRemaining);
            timerText.text = timeRemainingINT.ToString();
        }
        else
        {
            timeRemaining = 0;
                        int timeRemainingINT =  (int)(timeRemaining);

            TimerActivated = false;
            timerText.text = timeRemainingINT.ToString();
            OnTimerFinished();
        }
    }


    public void ShowAd(AdType adType)
    {
        if(adType == AdType.Video)
        {
            videoPlayer.PlayVideo();
            timeRemaining = videoDuration;

            TimerActivated = true;
        }
        if(adType == AdType.Image)
        {
            RawImage.SetActive(false);
            imageAdShower.ShowAdImage();
            timeRemaining = imageDuration;

            TimerActivated = true;
        }
    }



    public void OnCloseButtonClicked()
    {
        videoPlayer.StopVideo();
        adCanvas.SetActive(false);
    }

    public void OnTimerFinished()
    {
        CloseButton.SetActive(true);
    }
}


public enum AdType
{
    Video,
    Image
}