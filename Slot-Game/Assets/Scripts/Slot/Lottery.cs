using UnityEngine;


//内部抽選をするスクリプトです
public class Lottery : MonoBehaviour
{
    [SerializeField] private SlotProbabilityDataAccessor dataAccessor;

    // 役の確率を1つ取る
    float GetRate(Role role)
    {
        string key = role.ToString();

        if (dataAccessor.TryGetProbability(key, out float p))
        {
            return p;
        }

        Debug.LogWarning("確率データが見つかりません: " + key);
        return 0f;
    }

    // 全役の確率を1回だけ取る(順番は Role の並びと同じ)
    float[] LoadRates()
    {
        Role[] roles = (Role[])System.Enum.GetValues(typeof(Role));
        float[] rates = new float[roles.Length];

        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == Role.None)
            {
                rates[i] = 0f;
            }
            else
            {
                rates[i] = GetRate(roles[i]);
            }
        }

        return rates;
    }

    // 取っておいた確率で、乱数を1回引いて役を決める
    Role Pick(float[] rates)
    {
        Role[] roles = (Role[])System.Enum.GetValues(typeof(Role));
        float r = Random.value;

        for (int i = 0; i < roles.Length; i++)
        {
            if (roles[i] == Role.None)
            {
                continue;
            }

            if (r < rates[i])
            {
                return roles[i];
            }

            r -= rates[i];
        }

        return Role.None;
    }

    // 乱数を1回だけ引いて、当選役を決める
    public Role Draw(int settingNumber)
    {
        return Pick(LoadRates());
    }

    // 確率テスト(Inspector右上の︙から実行)
    [ContextMenu("Test Lottery")]
    void TestLottery()
    {
        float[] rates = LoadRates();    // 問い合わせはここで1回だけ

        int n = 100000;
        Role[] roles = (Role[])System.Enum.GetValues(typeof(Role));
        int[] count = new int[roles.Length];

        for (int i = 0; i < n; i++)
        {
            count[(int)Pick(rates)]++;
        }

        string msg = "";

        for (int i = 0; i < roles.Length; i++)
        {
            if (count[i] > 0)
            {
                msg += roles[i] + " 1/" + ((float)n / count[i]).ToString("F1") + "  ";
            }
            else
            {
                msg += roles[i] + " 1/-  ";
            }
        }

        Debug.Log(msg);
    }
}