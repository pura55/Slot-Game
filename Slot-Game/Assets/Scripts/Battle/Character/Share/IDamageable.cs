/// <summary>
/// ダメージを受けれるオブジェクトのインターフェース
/// </summary>
public interface IDamageable
{
    /// @briefダメージを受ける共通の関数
    /// @param amount ダメージ量
    public void Damaged(int amount);
}
