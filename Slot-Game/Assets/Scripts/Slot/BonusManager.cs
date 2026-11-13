using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private float pekaFirstRate = 0.5f;    // ボーナス当選時に先ペカになる割合
    [SerializeField] private GameObject pekaLamp;           // 金色に光らせるもの
    [SerializeField] private int bonusGames = 5;            // BIG_BONUS 後のブドウ確定ゲーム数(BONUS は半分)

    public Role bonusFlag = Role.None;                      // 持ち越し中のボーナス(BIG_BONUS / BONUS)
    public bool attackFlag;                                 // 戦闘側から true にされる AT 用フラグ

    int bonusGamesLeft;
    bool pekaAfterStop;

    // ボーナスに当選したとき
    public void OnRoleDrawn(Role role)
    {
        bool isBonusRole = role == Role.BIG_BONUS || role == Role.BONUS;

        if (isBonusRole && bonusFlag == Role.None)
        {
            bonusFlag = role;
            Debug.Log("ボーナス当選：" + bonusFlag);

            if (Random.value < pekaFirstRate)
            {
                SetPeka(true);                              // 先ペカ
            }
            else
            {
                pekaAfterStop = true;                       // 停止後ペカ
            }
        }
    }

    // ボーナス後のゲームなら、確定で出す役(ブドウ)を返す
    public bool TryTakeForcedRole(out Role role)
    {
        if (bonusGamesLeft > 0)
        {
            bonusGamesLeft--;
            Debug.Log("ボーナス中　残り" + bonusGamesLeft + "ゲーム");
            role = Role.COIN;
            return true;
        }

        role = Role.None;
        return false;
    }

    // 今、揃ってよいボーナス・ATの並びか
    public bool IsAllowedTriple(Symbol a, Symbol b, Symbol c)
    {
        if (SlotRules.MatchBonus(bonusFlag, a, b, c))
        {
            return true;
        }

        if (attackFlag && SlotRules.IsBarBarBar(a, b, c))
        {
            return true;
        }

        return false;
    }

    // 持っているボーナスが揃ったら、処理する
    public bool TryAlign(Symbol a, Symbol b, Symbol c)
    {
        if (!SlotRules.MatchBonus(bonusFlag, a, b, c))
        {
            return false;
        }

        Debug.Log("ボーナス入賞：" + bonusFlag);
        StartBonus(bonusFlag);
        bonusFlag = Role.None;
        SetPeka(false);
        return true;
    }

    // ATの並びが揃ったら、処理する
    public bool TryAlignAttack(Symbol a, Symbol b, Symbol c)
    {
        if (!attackFlag || !SlotRules.IsBarBarBar(a, b, c))
        {
            return false;
        }

        Debug.Log("アタックタイム突入");
        attackFlag = false;
        return true;
    }

    // 1ゲーム終わったとき(停止後ペカ)
    public void OnGameEnd()
    {
        if (pekaAfterStop)
        {
            pekaAfterStop = false;

            if (bonusFlag != Role.None)
            {
                SetPeka(true);
            }
        }
    }

    // 戦闘側から呼ぶ(敵をn体倒したとき)
    public void SetAttackFlag()
    {
        attackFlag = true;
    }

    void StartBonus(Role bonus)
    {
        if (bonus == Role.BIG_BONUS)
        {
            bonusGamesLeft = bonusGames;
        }
        else
        {
            bonusGamesLeft = Mathf.CeilToInt(bonusGames / 2f);
        }
    }

    void SetPeka(bool on)
    {
        if (pekaLamp != null)
        {
            pekaLamp.SetActive(on);
        }
    }
}