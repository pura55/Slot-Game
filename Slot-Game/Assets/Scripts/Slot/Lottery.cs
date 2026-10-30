using UnityEngine;


//内部抽選をするスクリプトです
public class Lottery : MonoBehaviour
{
    // 設定1〜6の確率(0番目が設定1)
    public RoleRates[] settings = new RoleRates[6];

    RoleRates Get(int settingNumber)
    {
        int i = Mathf.Clamp(settingNumber - 1, 0, settings.Length - 1);
        return settings[i];
    }

    void OnValidate()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            if (settings[i] != null && settings[i].NoneRate < 0f)
                Debug.LogWarning("設定" + (i + 1) + "の確率の合計が1.0を超えています");
        }
    }

    public Role Draw(int settingNumber)
    {
        RoleRates rates = Get(settingNumber);
        float r = Random.value;

        if (r < rates.seven) return Role.Seven;
        r -= rates.seven;

        if (r < rates.bar) return Role.Bar;
        r -= rates.bar;

        if (r < rates.animal) return Role.Animal;
        r -= rates.animal;

        if (r < rates.bell) return Role.Bell;
        r -= rates.bell;

        if (r < rates.grape) return Role.Grape;
        r -= rates.grape;

        if (r < rates.cherry) return Role.Cherry;
        r -= rates.cherry;

        if (r < rates.clown) return Role.Clown;

        return Role.None;
    }

    // TestLottery は今のままでOK(変更なし)
}