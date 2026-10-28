using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// バトルに参加できるキャラに搭載する機能を記載します
/// </summary>
public interface IBattleable
{
    GameObject GameObject { get; } // ゲームオブジェクトを返す

    List<IBattleable> EffectTargets { get; set; }

    event Action<IBattleable> OnDead; // 死亡時のイベント処理

    event Action<IBattleable> OnAct; // 行動するときのイベント処理

    /// @brief バトルを開始します
    void BattleStart();

    /// @brief 行動実行処理を行います
    void ExecuteAction();

    /// @brief 死亡時の処理を実行します
    void ExecuteDead();

    /// @brief オブジェクトをリセットする処理を行います
    void ObjectReset();
}
