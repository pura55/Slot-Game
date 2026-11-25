using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private void Start()
    {
        // バトル用シーンを追加で読み込む
        SceneManager.LoadSceneAsync("BattleScene", LoadSceneMode.Additive);
    }
}

