using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// ステージマネージャー
/// 
/// ステージの管理を行います
/// </summary>
public class StageManager : MonoBehaviour
{
    #region Config
    [SerializeField] private List<GameObject> stages; // Stage
    [SerializeField] private List<GameObject> stageBackGrounds; // 背景
    [SerializeField] private BattleManager battleManager; // バトルマネージャー
    [SerializeField] private GameObject player; // プレイヤー
    [SerializeField] private BattleFader battleFader; // バトルフェーダー
    [SerializeField] private Vector3 playerGeneratePosition = new Vector3(-7, 0f, 0f);
    [SerializeField] private float nextStageInterval = 1f; // 次のステージまでの間隔
    #endregion

    #region State
    private List<GameObject> gameObjects = new(); // 生成したステージや背景、プレイヤーの参照
    private const int initStageNumber = 0; // ステージの初期番号
    private int currentStageNumber = initStageNumber; // 現在のステージ番号
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateStage();
        battleFader.OrderFadeOut();
    }

    /// @brief 次のステージへ遷移する処理を行います
    public void ChangeNextStage()
    {
        battleFader.OrderFadeIn();

        StartCoroutine(WaitNextStage());
    }

    /// @brief ステージ遷移の待機をします
   IEnumerator WaitNextStage()
    {
        yield return new WaitForSeconds(nextStageInterval);

        currentStageNumber++;

        // 次のステージが存在しない場合最初のステージに遷移します
        if (stages.Count <= currentStageNumber)
        {
            currentStageNumber = initStageNumber;
        }

        // 逆順で安全にオブジェクトのリストを削除します
        // @brief リストの中身が空になるまで処理を実行
        // @param i カウントから1を減算して指数を求めます
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            GameObject destroyObject = gameObjects[i];
            gameObjects.Remove(destroyObject);
            Destroy(destroyObject);
        }

        GenerateStage();

        battleFader.OrderFadeOut();
    }

    /// @brief ステージを生成します
    /// @param stageNum ステージ番号
    private void GenerateStage()
    {
        GameObject backGround = Instantiate(stageBackGrounds[currentStageNumber], 
                                            stageBackGrounds[currentStageNumber].transform.position, 
                                            stageBackGrounds[currentStageNumber].transform.rotation);
        GameObject stage = Instantiate(stages[currentStageNumber], Vector3.zero, Quaternion.identity);
        WaveGenerator generator = stage.GetComponent<WaveGenerator>();
        StageMovement stageMovement = stage.GetComponent<StageMovement>();

        //if (generator != null)
        //{
        //    generator.Battle = battleManager;
        //}
        gameObjects.Add(backGround);
        gameObjects.Add(stage);

        GeneratePlayer(stageMovement);
    }

    /// @brief プレイヤーを生成します
    private void GeneratePlayer(StageMovement stageMovement)
    {
        GameObject generatePlayer = Instantiate(player, playerGeneratePosition, Quaternion.identity);
        PlayerManager playerManager = generatePlayer.GetComponent<PlayerManager>();
        playerManager.Movement = stageMovement;
        gameObjects.Add(generatePlayer);
    }
}
