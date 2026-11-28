using UnityEngine;
using UnityEngine.UI;

//キーボードの入力を、スロットの操作につなぐスクリプトです
public class SlotKeyInput : MonoBehaviour
{
    [SerializeField] private StopButton stopButton;
    [SerializeField] private Button betButton;
    [SerializeField] private CoinManager coinManager;

    [SerializeField] private KeyCode betKey = KeyCode.F;
    [SerializeField] private KeyCode leverKey = KeyCode.Space;
    [SerializeField] private KeyCode stopLeftKey = KeyCode.G;
    [SerializeField] private KeyCode stopCenterKey = KeyCode.H;
    [SerializeField] private KeyCode stopRightKey = KeyCode.J;

    void Update()
    {
        if(Input.GetKeyDown(betKey))
        {
            Bet();
        }

        if (Input.GetKeyDown(leverKey))
        {
            stopButton.StartAll();
        }

        if (Input.GetKeyDown(stopLeftKey))
        {
            stopButton.StopLeft();
        }

        if (Input.GetKeyDown(stopCenterKey))
        {
            stopButton.StopCenter();
        }

        if (Input.GetKeyDown(stopRightKey))
        {
            stopButton.StopRight();
        }
    }

    void Bet()
    {
        foreach(var r in stopButton.reels)
        {
            if(r.IsSpinning)
            {
                return;
            }
        }

        if(coinManager!=null&& coinManager.IsUsed)
        {
            return;
        }

        if (betButton != null)
        {
            betButton.onClick.Invoke();
        }
    }
}
