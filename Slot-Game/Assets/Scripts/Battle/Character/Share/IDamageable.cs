using System;
using UnityEngine;

/// <summary>
/// ダメージを受けれるオブジェクトのインターフェース
/// </summary>
public interface IDamageable
{
    /// @briefダメージを受ける共通の関数
    /// @param amount ダメージ量
    public void Damaged(int amount);

    // ダメージ表示用イベント
    public event Action<Vector3, int, DamagedTarget> OnDamaged;
}
