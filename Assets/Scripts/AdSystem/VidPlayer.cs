using UnityEngine;
using UnityEngine.Video;


public class VidPlayer : MonoBehaviour
{
    [SerializeField] private string VideoFileName;


    public void PlayVideo()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer) {
            string videoPath = System. IO. Path. Combine(Application.streamingAssetsPath, VideoFileName);
            Debug. Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
            }
    }

    public void StopVideo()
    {
        var videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer) {
            videoPlayer.Stop();

        }
    }

}