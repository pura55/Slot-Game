using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーコマンダー
/// 
/// プレイヤーの指示を行います
/// </summary>
public class PlayerCommander : MonoBehaviour
{
    #region Config
    [SerializeField] private List<PlayerCommand> playerCommands;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}