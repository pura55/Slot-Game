using UnityEngine;
using UnityEngine.EventSystems;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
