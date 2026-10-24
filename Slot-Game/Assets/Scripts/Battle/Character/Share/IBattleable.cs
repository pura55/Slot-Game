using System;
using UnityEngine;

/// <summary>
/// バトルに参加できるキャラに搭載する機能を記載します
/// </summary>
public interface IBattleable
{
    GameObject GameObject { get; } // ゲームオブジェクトを返す

    event Action<IBattleable> OnDead; // 死亡時のイベント処理

    /// @brief バトルを開始します
    void BattleStart();
}
