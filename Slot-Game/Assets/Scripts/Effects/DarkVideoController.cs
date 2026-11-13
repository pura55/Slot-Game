using UnityEngine;
using UnityEngine.Video;

public class DarkVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float startTime = 0.5f;

    public void PlayVideo()
    {
        videoPlayer.time = startTime;
        videoPlayer.Play();
    }
}