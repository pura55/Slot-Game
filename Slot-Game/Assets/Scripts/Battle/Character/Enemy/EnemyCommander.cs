using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーコマンダー
/// 
/// 敵ののコマンドを指示します
/// </summary>
public class EnemyCommander : MonoBehaviour
{
    #region Config
    [SerializeField] private List<EnemyCommand> enemyCommands; // 敵のコマンド
    [SerializeField] private SimpleEnemyAI enemyAI; // 敵AI
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    /// @brief コマンドを指令します
    public void OperateCommand(EnemyCommandType type)
    {
        enemyAI.ExecuteAction();

        // 指令されたコマンドが存在する場合実行します
        foreach (EnemyCommand command in enemyCommands)
        {
            if (command.enemyCommandType != type)
            {
                continue;
            }

            command.Execute(gameObject, GetPlayer());
            return;
        }
    }

    /// @brief 参加している敵を取得します
    private List<GameObject> GetPlayer()
    {
        List<GameObject> enemies = new List<GameObject>();

        // 参加している敵を追加します
        foreach (IBattleable joinTarget in enemyAI.EffectTargets)
        {
            enemies.Add(joinTarget.GameObject);
        }

        return enemies;
    }
}
