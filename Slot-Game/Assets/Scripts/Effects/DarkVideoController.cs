using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;

public class DarkVideoController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private PlayableDirector director;

    [Header("Ø’f—p‚Ì‰æ‘œ")]
    [SerializeField] private GameObject blackTop;
    [SerializeField] private GameObject blackBottom;

    void Start()
    {
        blackTop.SetActive(false);
        blackBottom.SetActive(false);

        videoPlayer.prepareCompleted += OnPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Prepare();
    }

    private void OnPrepared(VideoPlayer player)
    {
        player.Play();
    }

    private void OnVideoFinished(VideoPlayer player)
    {
        // Ø’f—p‚Ì‰æ‘œ‚ğ•\¦
        blackTop.SetActive(true);
        blackBottom.SetActive(true);

        // “®‰æ‚ğ’â~
        player.Stop();

        // Timeline‚ğÅ‰‚©‚çÄ¶
        director.time = 0;
        director.Evaluate();
        director.Play();

        Debug.Log("Ø’f‰‰oŠJn");
    }

    private void OnDestroy()
    {
        videoPlayer.prepareCompleted -= OnPrepared;
        videoPlayer.loopPointReached -= OnVideoFinished;
    }
}