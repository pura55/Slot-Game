using System;
using UnityEngine;

/// <summary>
/// バトルに参加できるキャラに搭載する機能を記載します
/// </summary>
public interface IBattlable
{
    event Action<GameObject> OnJoined; // バトルに参加する際のイベント通知
    event Action<GameObject> OnLeft; // バトルから退出する際のイベント通知

    bool InBattle { get; set; } // バトル時かどうかのフラグ取得・設定
}
