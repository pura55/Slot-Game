
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;

public class DarkVideoController : MonoBehaviour
{
    [Header("ˆÃ“]“®‰æ")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private PlayableDirector director;

    [Header("Ø’f—p‚Ì‰æ‘œ")]
    [SerializeField] private GameObject blackTop;
    [SerializeField] private GameObject blackBottom;

    [Header("aŒ‚ƒGƒtƒFƒNƒg")]
    [SerializeField] private GameObject slashTop;
    [SerializeField] private GameObject slashBottom;

    [Header("aŒ‚‚ğL‚Î‚·")]
    [SerializeField] private SlashStretch slashTopStretch;
    [SerializeField] private SlashStretch slashBottomStretch;

    private void Start()
    {
        blackTop.SetActive(false);
        blackBottom.SetActive(false);

        slashTop.SetActive(false);
        slashBottom.SetActive(false);

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
        // “®‰æ‚ğ’â~
        player.Stop();

        // Ø’f—p‰æ‘œ‚ğ•\¦
        blackTop.SetActive(true);
        blackBottom.SetActive(true);

        // aŒ‚‚ğ•\¦
        slashTop.SetActive(true);
        slashBottom.SetActive(true);

        // ¶‰º‚©‚ç‰Eã‚ÖL‚Î‚·
        slashTopStretch.PlayStretch();
        slashBottomStretch.PlayStretch();

        // Timeline‚ğÅ‰‚©‚çÄ¶
        director.time = 0;
        director.Evaluate();
        director.Play();

        Debug.Log("Ø’f‰‰oŠJn");
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnPrepared;
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
