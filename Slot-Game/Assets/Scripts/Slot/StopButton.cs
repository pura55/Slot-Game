using UnityEngine;

//リールを止めるためのボタンのスクリプトです
public class StopButton : MonoBehaviour
{
    public Reelcontroller[] reels;
    public void StartAll()
    {
        foreach(var reel in reels)reel.StartSpin();
    }

    public void StopLeft() { reels[0].StopSpin(); }
    public void StopCenter() {  reels[1].StopSpin(); }
    public void StopRight() { reels[2].StopSpin(); }
}
