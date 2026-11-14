using UnityEngine;

// ストップを押したとき、どこで止めるかを決めるスクリプトです
public class ReelStopSolver : MonoBehaviour
{
    public int maxSlip = 4;         // 小役を引き込める最大コマ数
    public int bonusMaxSlip = 2;    // ボーナスを引き込める最大コマ数(小さいほど目押しが必要)

    // 押した位置から0〜maxSlipコマ滑らせて、いちばん点数が高い位置を選ぶ
    public int ChooseSlip(Reelcontroller[] reels, int reel, int press, Role currentRole,
                          BonusManager bonus, int[] stopIndex, bool[] decided)
    {
        int n = reels[reel].N;
        int bestIndex = press;
        int bestScore = int.MinValue;

        for (int d = 0; d <= maxSlip; d++)
        {
            int idx = SlotRules.Mod(press + d, n);
            bool bonusPull = d <= bonusMaxSlip;     // この滑り量なら、ボーナスを引き込んでよいか
            int score = Evaluate(reels, reel, idx, currentRole, bonus, stopIndex, decided, bonusPull);

            // 同点なら、滑りが少ないほうを残す
            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = idx;
            }
        }

        return bestIndex;
    }

    // reel番目をidxで止めたときの点数
    int Evaluate(Reelcontroller[] reels, int reel, int idx, Role currentRole,
                 BonusManager bonus, int[] stopIndex, bool[] decided, bool bonusPull)
    {
        int[] cur = (int[])stopIndex.Clone();
        bool[] st = (bool[])decided.Clone();
        cur[reel] = idx;
        st[reel] = true;

        Symbol target;
        bool hasTarget = SlotRules.TryGetTarget(currentRole, out target);

        // ボーナスを引き込むのは、小役が当選していないゲームだけ
        Symbol[] pattern = null;
        bool hasBonus = false;

        if (bonusPull && !hasTarget)
        {
            hasBonus = SlotRules.TryGetBonusPattern(bonus.bonusFlag, out pattern);
        }

        int score = 0;

        foreach (var line in SlotRules.Lines)
        {
            Symbol[] s = new Symbol[3];
            bool allStopped = true;
            bool alive = hasTarget;         // 小役が、まだ揃う可能性がある
            bool bonusAlive = hasBonus;     // ボーナスが、まだ揃う可能性がある

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

                if (hasBonus && s[r] != pattern[r])
                {
                    bonusAlive = false;
                }
            }

            if (alive)
            {
                score += 1;
            }

            if (bonusAlive)
            {
                score += 1;
            }

            // 3つ止まって、入賞する並びができた
            if (allStopped && SlotRules.IsWinningTriple(s[0], s[1], s[2]))
            {
                if (IsAllowed(currentRole, bonus, s[0], s[1], s[2]))
                {
                    if (hasTarget)
                    {
                        score += 100;
                    }
                    else if (hasBonus && SlotRules.MatchBonus(bonus.bonusFlag, s[0], s[1], s[2]))
                    {
                        score += 100;       // ボーナスが揃う
                    }
                }
                else
                {
                    score -= 1000;          // 当選していない役が揃う → 蹴飛ばす
                }
            }
        }

        // HEAL は左リールに図柄が見えるか
        if (reel == 0)
        {
            bool visible = false;

            for (int row = 0; row < 3; row++)
            {
                if (reels[0].SymbolAt(idx, row) == SlotRules.HealSymbol)
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
                    score -= 500;           // 当選していないのに出る → 蹴飛ばす
                }
            }
        }

        return score;
    }

    // 今、揃ってよい並びか
    bool IsAllowed(Role currentRole, BonusManager bonus, Symbol a, Symbol b, Symbol c)
    {
        Symbol t;

        if (SlotRules.TryGetTarget(currentRole, out t))
        {
            if (a == t && b == t && c == t)
            {
                return true;
            }
        }

        return bonus.IsAllowedTriple(a, b, c);
    }

    // 押した位置から、その図柄が見えるまで滑れるか
    public bool CanReach(Reelcontroller reel, int press, Symbol t)
    {
        for (int d = 0; d <= maxSlip; d++)
        {
            for (int row = 0; row < 3; row++)
            {
                if (reel.SymbolAt(SlotRules.Mod(press + d, reel.N), row) == t)
                {
                    return true;
                }
            }
        }

        return false;
    }
}