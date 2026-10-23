using UnityEngine;

//リールを止めるためのボタンのスクリプトです
public class StopButton : MonoBehaviour
{
    public Reelcontroller[] reels;

    //スタートボタン
    public void StartAll()
    {
        foreach(var reel in reels)reel.StartSpin();
    }

    //ストップボタン
    public void StopLeft() { reels[0].StopSpin(); }
    public void StopCenter() {  reels[1].StopSpin(); }
    public void StopRight() { reels[2].StopSpin(); }
}
