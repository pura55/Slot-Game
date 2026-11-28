using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// ゲームマネージャー
/// 
/// ゲーム管理を行います
/// </summary>
public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RemoveDuplicateEventSystem();
        PlayerLevelManager.InitLevel();
    }

    // Update is called once per frame
    void Update()
    {
        ChangePlayMode();
    }

    /// @brief 重複したEventSystemを削除します
    private void RemoveDuplicateEventSystem()
    {
        // 現在のEventSystemを取得
        EventSystem current = EventSystem.current;

        // 全EventSystemを取得
        EventSystem[] all = FindObjectsOfType<EventSystem>();

        // 重複があれば削除
        foreach (EventSystem es in all)
        {
            if (es != current)
            {
                Debug.Log("EventSystemを削除します: " + es.name);
                Destroy(es.gameObject);
            }
        }
    }

    /// @brief プレイする際のモードを変更します
    private void ChangePlayMode()
    {
        if(Keyboard.current.dKey.wasPressedThisFrame && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if(!DebugManager.IsDebug)
            {
                DebugManager.IsDebug = true;
                Debug.Log("デバックモードに入りました。");
            }
            else
            {
                DebugManager.IsDebug = false;
                Debug.Log("デバックモードを解除しました。");
            }
        }
    }

    /// @brief タイトルシーンのロードを行います
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
