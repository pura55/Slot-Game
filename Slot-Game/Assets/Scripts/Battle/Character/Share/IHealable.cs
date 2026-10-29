
/// <summary>
/// 回復を受けれるオブジェクトのインターフェース
/// </summary>
public interface IHealable
{
    /// @brief 回復をする共通の関数
    /// @param amount 回復量
    public void Healed(int amount);
}
