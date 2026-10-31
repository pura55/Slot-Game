using UnityEngine;
using UnityEngine.UI;


//リールをコントロールする用のスクリプトです
public class Reelcontroller : MonoBehaviour
{
    public RawImage image;                  //回すリール
    public Symbol[] layout = new Symbol[21];
    public float speed = 1.3f;              //1秒あたり何週

    public string layoutText;               //並びを文字で貼り入れるための欄

    [SerializeField] private NumberManager numberManager;

    bool spinning;
    bool stopping;                          //停止位置へ滑っている途中
    int targetIndex;                        //滑って止まる位置
    float pos;                              //0～1の現在位置

    Symbol[] Layout {  get { return layout; } }         //このリールの図柄の並び

    public int N { get { return Layout.Length; } }      //コマ数（配列の長さから自動で決まる）
    public bool IsSpinning { get { return spinning; } }
    public bool IsStopping { get { return stopping; } }

    void Start()
    {
        Apply();
    }

    //リールを回す
    public void StartSpin()
    {
        // 3枚消費していなければ回さない
        if (numberManager == null || !numberManager.IsUsed)
        {
            Debug.Log("先に3枚消費してください");
            return;
        }

        stopping = false;
        spinning = true;
    }

    //ボタンを押した瞬間先にある最初のコマの番号
    public int PressIndex()
    {
        return Mod(Mathf.CeilToInt(pos * N),N);
    }

    //指定したコマの位置まで回り続けてから止める
    public void RequestStop(int index)
    {
        if(!spinning || stopping)
        {
            return;
        }

        targetIndex = index;
        stopping = true;
    }
    
    //index番目で止まったときの段の図柄
    public Symbol SymbolAt(int index,int row)
    {
        return Layout[Mod(N - 3 -index + row,N)];
    }

    //現在の停止位置の図柄
    public Symbol GetSymbol(int row)
    {
        int k = Mod(Mathf.RoundToInt(pos * N),N);
        return SymbolAt(k,row);
    }

    static int Mod(int a, int m)
    {
        return ((a % m) + m) % m;
    }

    void Update()
    {
        if (!spinning)
        {
            return;
        }

        float delta = speed * Time.deltaTime;

        if (stopping)
        {
            float target = targetIndex / (float)N;
            float dist = Mathf.Repeat(target = pos, 1f);

            if (dist <= delta)
            {
                pos = target;
                spinning = false;
                stopping = false;
                Apply();
                return;
            }
        }

        pos = Mathf.Repeat(pos + delta, 1f);
        Apply();
    }

    void Apply()
    {
        var r = image.uvRect;
        r.y = pos;
        r.height = 3f / N;        //窓に3コマ表示
        image.uvRect = r;
    }

    [ContextMenu("File Layout From Text")]
    void FillLayoutFromText()
    {
        string[] names = layoutText.Split(new char[] { ' ', ',', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        layout = new Symbol[names.Length];

        for(int i = 0; i< names.Length;  i++)
        {
            layout[i] = (Symbol)System.Enum.Parse(typeof(Symbol),names[i]);
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
