using UnityEngine;

//リールを止めるためのボタンのスクリプトです
public class StopButton : MonoBehaviour
{
    public Reelcontroller[] reels;
    public Lottery lottery;
    [Range(1,6)] public int setting = 1;
    public Role currentRole;

    public int maxSlip = 4;                             //最大で滑るコマ数
    public float pekaFirstRate = 0.5f;                  //ボーナス当選時に先ペカになる割合
    public GameObject pekaLamp;                         //金色に光らせるもの

    public Role bonusFlag = Role.None;                  //持ち越し中のボーナス(BIG_BONUS / BONUS)
    public bool attackFlag;                             //戦闘側からtrueにされるAT用フラグ

    //テスト用: ONにすると抽選せず、指定した役で固定する
    public bool useDebugRole;
    public Role debugRole;

    static readonly Symbol HealSymbol = Symbol.Cherry;  // HEAL(左リール)の図柄

    //見る3ライン
    static readonly int[][] Lines =
    {
        new int[] { 1, 1, 1 },                          // 中段
        new int[] { 0, 1, 2 },                          // ななめ下がり
        new int[] { 2, 1, 0 }                           // ななめ上がり
    };

    int[] stopIndex = new int[3];
    bool[] decided = new bool[3];
    bool waitJudge;
    bool pekaAfterStop;

    static bool TryGetTarget(Role role, out Symbol target)
    {
        switch (role)
        {
            case Role.COIN:
                target = Symbol.Grape;
                return true;
            case Role.BIG_COIN:
                target = Symbol.Clown;
                return true;
            case Role.LIGHT_ATTACK:
                target = Symbol.Bell;
                return true;
            case Role.REPLAY:
                target = Symbol.Animal;
                return true;
        }

        target = Symbol.Bar;
        return false;
    }

    //-------スタート・ストップ-------

    public void StartAll()
    {
        foreach (var reel in reels)
        {
            if (reel.IsSpinning)
            {
                return;
            }
        }

        if (useDebugRole)
        {
            currentRole = debugRole;
        }
        else
        {
            currentRole = lottery.Draw(setting);
        }

        Debug.Log("当選役：" + currentRole);

        //ボーナス当選
        bool isBonusRole = currentRole == Role.BIG_BONUS || currentRole == Role.BONUS;

        if (isBonusRole && bonusFlag == Role.None)
        {
            bonusFlag = currentRole;
            Debug.Log("ボーナス当選：" + bonusFlag);

            if (Random.value < pekaFirstRate)
            {
                SetPeka(true);                              //先ぺか
            }
            else
            {
                pekaAfterStop = true;                       //停止後ぺか
            }
        }

        for (int i = 0; i < 3; i++)
        {
            decided[i] = false;
        }

        foreach (var r in reels)
        {
            r.StartSpin();
        }

        waitJudge = reels[0].IsSpinning;
    }

    public void StopLeft() { StopReel(0); }
    public void StopCenter() { StopReel(1); }
    public void StopRight() { StopReel(2); }

    void StopReel(int i)
    {
        if (!reels[i].IsSpinning || reels[i].IsStopping)
        {
            return;
        }

        int press = reels[i].PressIndex();
        int best = ChooseSlip(i, press);

        decided[i] = true;
        stopIndex[i] = best;
        reels[i].RequestStop(best);
    }

    void Update()
    {
        if (!waitJudge)
        {
            return;
        }

        foreach (var r in reels)
        {
            if (r.IsSpinning)
            {
                return;
            }
        }

        waitJudge = false;
        Judge();

        //停止後ぺか
        if (pekaAfterStop)
        {
            pekaAfterStop = false;
            if (bonusFlag != Role.None)
            {
                SetPeka(true);
            }
        }
    }

    void SetPeka(bool on)
    {
        if (pekaLamp != null)
        {
            pekaLamp.SetActive(on);
        }
    }

    //-------停止制御-------

    static int Mod(int a, int m)
    {
        return ((a % m) + m) % m;
    }

    int ChooseSlip(int reel, int press)
    {
        int n = reels[reel].N;
        int bestIndex = press;
        int bestScore = int.MinValue;

        for (int d = 0; d <= maxSlip; d++)
        {
            int idx = Mod(press + d, n);
            int score = Evaluate(reel, idx);

            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = idx;
            }
        }

        return bestIndex;
    }

    //reel番目をidxで止めたときの点数
    int Evaluate(int reel, int idx)
    {
        int[] cur = (int[])stopIndex.Clone();
        bool[] st = (bool[])decided.Clone();
        cur[reel] = idx;
        st[reel] = true;

        Symbol target;
        bool hasTarget = TryGetTarget(currentRole, out target);
        int score = 0;

        foreach (var line in Lines)
        {
            Symbol[] s = new Symbol[3];
            bool allStopped = true;
            bool alive = hasTarget;     //まだ揃う可能性がある

            for (int r = 0; r < 3; r++)
            {
                if (!st[r])
                {
                    allStopped = false;
                    continue;
                }

                s[r] = reels[r].SymbolAt(cur[r], line[r]);

                if (s[r] != target)
                {
                    alive = false;
                }
            }

            if (alive)
            {
                score += 1;
            }

            //3つ止まって、入賞する並びができた
            if (allStopped && IsWinningTriple(s[0], s[1], s[2]))
            {
                if (IsAllowed(s[0], s[1], s[2]))
                {
                    if (hasTarget)
                    {
                        score += 100;
                    }
                }
                else
                {
                    score -= 1000;      //当選していない役が揃う → 蹴飛ばす
                }
            }
        }

        //HEALは左リールに図柄が見えるか
        if (reel == 0)
        {
            bool visible = false;

            for (int row = 0; row < 3; row++)
            {
                if (reels[0].SymbolAt(idx, row) == HealSymbol)
                {
                    visible = true;
                }
            }

            if (currentRole == Role.HEAL)
            {
                if (visible)
                {
                    score += 500;
                }
            }
            else
            {
                if (visible)
                {
                    score -= 500;       //当選していないのに出る → 蹴飛ばす
                }
            }
        }

        return score;
    }

    static bool IsWinningTriple(Symbol a,Symbol b ,Symbol c)
    {
        if(a == b && b == c)
        {
            return true;
        }

        return a == Symbol.Seven && b == Symbol.Seven && c == Symbol.Bar;
    }

    static bool MatchBonus(Role bonus, Symbol a, Symbol b,Symbol c)
    {
        switch (bonus)
        {
            case Role.BIG_BONUS:
                return a == Symbol.Seven && b == Symbol.Seven && c == Symbol.Seven;
            case Role.BONUS:
                return a == Symbol.Seven && b == Symbol.Seven && c == Symbol.Bar;
        }
        return false;
    }

    static bool IsBarBarBar(Symbol a, Symbol b, Symbol c)
    {
        return a == Symbol.Bar && b == Symbol.Bar && c == Symbol.Bar;
    }

    //今、揃ってよい並びか
    bool IsAllowed(Symbol a, Symbol b, Symbol c)
    {
        Symbol t;

        if (TryGetTarget(currentRole, out t))
        {
            if (a == t && b == t && c == t)
            {
                return true;
            }
        }

        if (MatchBonus(bonusFlag, a, b, c))
        {
            return true;
        }

        if (attackFlag && IsBarBarBar(a, b, c))
        {
            return true;
        }

        return false;
    }

    //-------入賞判定-------

    void Judge()
    {
        Symbol target;
        bool hasTarget = TryGetTarget(currentRole, out target);
        bool hit = false;

        foreach (var line in Lines)
        {
            Symbol a = reels[0].GetSymbol(line[0]);
            Symbol b = reels[1].GetSymbol(line[1]);
            Symbol c = reels[2].GetSymbol(line[2]);

            if (!IsWinningTriple(a, b, c))
            {
                continue;
            }

            Debug.Log("入賞: " + a + " " + b + " " + c + "  (当選役: " + currentRole + ")");

            if (hasTarget && a == target && b == target && c == target)
            {
                hit = true;
            }

            if (MatchBonus(bonusFlag, a, b, c))
            {
                Debug.Log("ボーナス入賞: " + bonusFlag);
                bonusFlag = Role.None;
                SetPeka(false);
            }

            if (attackFlag && IsBarBarBar(a, b, c))
            {
                Debug.Log("アタックタイム突入");
                attackFlag = false;
            }
        }

        if (hasTarget && !hit)
        {
            Debug.Log("取りこぼし: " + currentRole);
        }

        for (int row = 0; row < 3; row++)
        {
            if (reels[0].GetSymbol(row) == HealSymbol)
            {
                Debug.Log("入賞: HEAL(左リール)  (当選役: " + currentRole + ")");
                break;
            }
        }
    }

    //-------引き込めるか-------
    [ContextMenu("Check Reach")]
    void CheckReach()
    {
        Symbol[] targets =
        {
            Symbol.Grape, Symbol.Animal, Symbol.Bell, Symbol.Clown, Symbol.Cherry
        };

        for (int r = 0; r < reels.Length; r++)
        {
            int n = reels[r].N;
            string msg = "リール" + (r + 1) + "  ";

            foreach (Symbol t in targets)
            {
                int ok = 0;

                for (int p = 0; p < n; p++)
                {
                    if (CanReach(reels[r], p, t))
                    {
                        ok++;
                    }
                }

                msg += t + " " + (100 * ok / n) + "%  ";
            }

            Debug.Log(msg);
        }
    }

    bool CanReach(Reelcontroller reel, int press, Symbol t)
    {
        for (int d = 0; d <= maxSlip; d++)
        {
            for (int row = 0; row < 3; row++)
            {
                if (reel.SymbolAt(Mod(press + d, reel.N), row) == t)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
