using UnityEngine;
using UnityEngine.UI;


//リールをコントロールする用のスクリプトです
public class Reelcontroller : MonoBehaviour
{
    public RawImage image;                  //回すリール
    public int symbolCount = 21;            //画像のコマ数
    public float speed = 1.5f;              //1秒あたり何週

    bool spinning;
    float pos;                              // 0～1の現在位置
    void Start()
    {
        Apply();
        //StartSpin();                      //これでまわるよ
    }

    public void StartSpin()
    {
        spinning = true;
    }

    public void StopSpin()
    {
        spinning = false;
        float step = 1f / symbolCount;      //コマの境目にぴったり合わせる
        pos = Mathf.Round(pos / step) * step;
        Apply();
    }

    void Update()
    {
        if (!spinning) return;
        pos = Mathf.Repeat(pos + speed * Time.deltaTime, 1f);
        Apply();
    }

    void Apply()
    {
        var r = image.uvRect;
        r.y = pos;
        r.height = 3f / symbolCount;        //窓に3コマ表示
        image.uvRect = r;
    }
}
