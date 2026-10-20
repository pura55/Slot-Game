using System;
using UnityEngine;

/// <summary>
/// バトルに参加できるキャラに搭載する機能を記載します
/// </summary>
public interface IBattlable
{
    event Action<GameObject> OnJoined;
    event Action<GameObject> OnLeft;

    bool InBattle { get; set; }
}
