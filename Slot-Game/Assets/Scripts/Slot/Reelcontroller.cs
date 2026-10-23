using UnityEngine;
using UnityEngine.UI;


//リールをコントロールする用のスクリプトです
public class Reelcontroller : MonoBehaviour
{
    public RawImage image;                  //回すリール
    //public ReelLayoutData layoutData;       //リール配列のデータファイル
    //public int reelIndex;                   //0=左、1=中、2=右
    public Symbol[] layout = new Symbol[21];
    public float speed = 1.5f;              //1秒あたり何週

    bool spinning;
    float pos;                              // 0～1の現在位置

    [SerializeField] private NumberManager numberManager;

    //このリールの図柄の並び
    //Symbol[] Layout { get {  return layoutData.Get(reelIndex); } }
    Symbol[] Layout {  get { return layout; } }
    //コマ数（配列の長さから自動で決まる）
    int N { get { return Layout.Length; } } 

    //回転中か
    public bool IsSpinning { get { return spinning; } }

    void Start()
    {
        Apply();
    }

    public void StartSpin()
    {
        // 3枚消費していなければ回さない
        if (numberManager == null || !numberManager.IsUsed)
        {
            Debug.Log("先に3枚消費してください");
            return;
        }

        spinning = true;
    }

    public void StopSpin()
    {
        if (!spinning) return;
        spinning = false;
        pos = StopIndex() / (float)N;     //コマの境目にぴったり合わせる
        Apply();
    }

    //現在の停止位置
    int StopIndex()
    {
        return Mod(Mathf.RoundToInt(pos * N), N);
    }

    public Symbol GetSymbol(int row)
    {
        int k = StopIndex();
        return Layout[Mod(N - 3 - k + row,N)];
    }

    static int Mod(int a, int m)
    {
        return ((a % m) + m) % m;
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
        r.height = 3f / N;        //窓に3コマ表示
        image.uvRect = r;
    }
}
