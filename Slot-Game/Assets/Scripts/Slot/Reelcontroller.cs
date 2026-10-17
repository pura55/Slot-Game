using UnityEngine;


//リールをコントロールする用のスクリプトです
public class Reelcontroller : MonoBehaviour
{
    //リールを取得
    public RectTransform Reel1;
    public RectTransform Reel2;
    public RectTransform Reel3;

    //初期位置
    private Vector2 initalpos1;
    private Vector2 initalpos2;
    private Vector2 initalpos3;

    //リールの回転速度
    private float speed1 = 500f;
    private float speed2 = 480f;
    private float speed3 = 460f;

    void Start()
    {
        initalpos1 = Reel1.anchoredPosition;
        initalpos2 = Reel2.anchoredPosition;
        initalpos3 = Reel3.anchoredPosition;
    }

    private void Update()
    {
        MoveReel(Reel1, speed1, initalpos1);
        MoveReel(Reel2, speed2, initalpos2);
        MoveReel(Reel3, speed3, initalpos3);
    }

    void MoveReel(RectTransform reel,float speed,Vector2 initialPos)
    {
        Vector2 pos = reel.anchoredPosition;
        pos.y -= speed * Time.deltaTime;
        if(pos.y < -1500f)
        {
            pos = initialPos;
        }

        reel.anchoredPosition = pos;
    }
}
