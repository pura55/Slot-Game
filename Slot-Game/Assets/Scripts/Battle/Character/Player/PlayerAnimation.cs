using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// プレイヤーアニメーション
/// 
/// プレイヤーのアニメーション処理を行います
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    #region Config
    [Header("ASSETS")]
    [SerializeField] private Sprite[] idleAnimation; // 待機アニメーション
    [SerializeField] private Sprite[] walkAnimation; // 移動アニメーション

    [Header("RENDERER")]
    [SerializeField] private SpriteRenderer spriteRenderer; // スプライトレンダラー

    [SerializeField] private float secondPerSprite = 0.5f; // １スプライトの更新にかかる時間
    #endregion

    #region State
    private int spriteIndex = 0; // スプライト番号
    private PlayerState playerState = PlayerState.IDLE;
    #endregion

    void Start()
    {
        // 自動停止用トークンを渡す
        var token = this.GetCancellationTokenOnDestroy();

        // ループ処理を起動
        UpdateSpriteLoopAsync(token).Forget();
    }

    /// @brief ループでスプライトを更新し続ける関数
    /// @param token オブジェクトが削除された際に関数を停止するためのトークン
    private async UniTaskVoid UpdateSpriteLoopAsync(CancellationToken token)
    {
        // ループから抜けるためにトークンを監視
        while (!token.IsCancellationRequested)
        {
            // 時間分待機して、新しいスプライトを受け取る
            spriteRenderer.sprite = await WaitChangeTimeAsync(token);
        }
    }

    /// @brief スプライトの変更を待機する関数 
    /// @param token オブジェクトが削除された際に関数を停止するためのトークン
    private async UniTask<Sprite> WaitChangeTimeAsync(CancellationToken token)
    {

        //// 時間分待機します
        //await UniTask.DelayFrame(TimeSpan.FromSeconds(secondPerSprite));

        float timer = 0f;

        // 指定秒数経過まで、毎フレーム待機を繰り返す
        while (timer < secondPerSprite)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, token);

            timer += Time.deltaTime;
        }

        spriteIndex++;
        if(idleAnimation.Length <= spriteIndex)
        {
            spriteIndex = 0;
        }

        return idleAnimation[spriteIndex]; 
    }
}
