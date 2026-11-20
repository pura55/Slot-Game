using UnityEngine;

//スロット全体の流れ(スタート・ストップ・判定)を管理するスクリプトです
public class StopButton : MonoBehaviour
{
    [Header("つなぐもの")]
    public Reelcontroller[] reels;                          //左・中・右の順
    public Lottery lottery;
    [SerializeField] private ReelStopSolver solver;
    [SerializeField] private BonusManager bonus;
  
    [SerializeField] private NumberManager numberManager;

    [Header("テスト用")]
    public bool ignoreBattleState;

    [Header("設定")]
    [Range(1, 6)] public int setting = 1;

    [Header("テスト用(ONにすると抽選せず、指定した役で固定)")]
    public bool useDebugRole;
    public Role debugRole;

    public Role currentRole;                                //今回の当選役

    // 戦闘側が受け取るイベント
    public event System.Action<PlayerCommandType> OnHit;

    int[] stopIndex = new int[3];
    bool[] decided = new bool[3];
    bool waitJudge;

    //---------------- スタート・ストップ ----------------

    public void StartAll()
    {
        foreach (var r in reels)
        {
            if (r.IsSpinning)
            {
                return;
            }
        }

        if(!ignoreBattleState&&
            BattleStateObserver.PlayerBattleState == CharacterBattleState.IDLE &&
            BattleStateObserver.EnemyBattleState == CharacterBattleState.IDLE)
        {
            Debug.Log("今はレバーを押せない");
            return;
        }

        //BETしていなければ始めない
        if (numberManager != null && !numberManager.IsUsed)
        {
            Debug.Log("先にBETしてください");
            return;
        }

        //当選役を決める(ボーナス後のゲームは、ブドウが確定)
        Role forced;

        if (bonus.TryTakeForcedRole(out forced))
        {
            currentRole = forced;
        }
        else if (useDebugRole)
        {
            currentRole = debugRole;
        }
        else
        {
            currentRole = lottery.Draw(setting);
        }

        Debug.Log("当選役：" + currentRole);

        bonus.OnRoleDrawn(currentRole);

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

    public void StopLeft()   { StopReel(0); }
    public void StopCenter() { StopReel(1); }
    public void StopRight()  { StopReel(2); }

    void StopReel(int i)
    {
        //回っていない、または止まりに行っている最中は受け付けない
        if (!reels[i].IsSpinning || reels[i].IsStopping)
        {
            return;
        }

        //左から順に押していなければ、受け付けない
        for (int j = 0; j < i; j++)
        {
            if (!decided[j])
            {
                return;
            }
        }

        int press = reels[i].PressIndex();
        int best = solver.ChooseSlip(reels, i, press, currentRole, bonus, stopIndex, decided);

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
        bonus.OnGameEnd();
    }

    //---------------- 入賞判定 ----------------

    void Judge()
    {
        Symbol target;
        bool hasTarget = SlotRules.TryGetTarget(currentRole, out target);
        bool hit = false;
        bool bonusAligned = false;

        foreach (var line in SlotRules.Lines)
        {
            Symbol a = reels[0].GetSymbol(line[0]);
            Symbol b = reels[1].GetSymbol(line[1]);
            Symbol c = reels[2].GetSymbol(line[2]);

            if (!SlotRules.IsWinningTriple(a, b, c))
            {
                continue;
            }

            Debug.Log("入賞: " + a + " " + b + " " + c + "  (当選役: " + currentRole + ")");

            if (hasTarget && a == target && b == target && c == target)
            {
                hit = true;
            }

            if (bonus.TryAlign(a, b, c))
            {
                bonusAligned = true;
            }

            bonus.TryAlignAttack(a, b, c);
        }

        if (hasTarget && !hit)
        {
            Debug.Log("取りこぼし: " + currentRole);
        }

        // HEAL は左リールに図柄が見えるか
        bool healHit = false;

        for (int row = 0; row < 3; row++)
        {
            if (reels[0].GetSymbol(row) == SlotRules.HealSymbol)
            {
                healHit = true;
                break;
            }
        }

        // 戦闘側へ送る(1ゲームに1回、結果を送る)
        if (hasTarget && hit)
        {
            Fire(SlotRules.ToCommand(currentRole));
        }
        else if (currentRole == Role.HEAL && healHit)
        {
            Fire(PlayerCommandType.HEAL);
        }
        else
        {
            Fire(PlayerCommandType.NONE);
        }

        if (bonusAligned)
        {
            Fire(PlayerCommandType.BONUS);
        }
    }

    void Fire(PlayerCommandType command)
    {
        Debug.Log("OnHit: " + command);
        OnHit?.Invoke(command);
    }

    //---------------- 引き込めるかの確認 ----------------

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
                    if (solver.CanReach(reels[r], p, t))
                    {
                        ok++;
                    }
                }

                msg += t + " " + (100 * ok / n) + "%  ";
            }

            Debug.Log(msg);
        }
    }
}