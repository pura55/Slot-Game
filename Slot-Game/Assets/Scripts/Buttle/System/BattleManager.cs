using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// バトルマネージャー
/// 
/// バトルを管理します
/// </summary>
public class BattleManager : MonoBehaviour
{
    #region State
    private BattleState battleState = BattleState.IDLE; // バトルの状態
    private IBattlable joinTarget; // 参加対象
    private IBattlable leaveTarget; // 退場対象
    private List<GameObject> joinCharacter = new(); // 参加しているキャラ
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// @brief 状態設定を行います
    public void SetState(BattleStateInput input)
    {
        battleState = (battleState, input) switch
        {
            (BattleState.IDLE, BattleStateInput.START) => BattleState.RUNNING,
            (BattleState.RUNNING, BattleStateInput.END) => BattleState.FINISHING,
            _ => battleState
        };

        OnEnterState(battleState);
    }

    /// @brief 各状態に入った際の処理を行います
    private void OnEnterState(BattleState state)
    {
        switch(state)
        {
            case BattleState.IDLE:
                break;
            case BattleState.RUNNING:
                break;
            case BattleState.FINISHING:
                break;
        }
    }

    /// @brief バトルに入る対象を設定します
    public void SetJoinTarget(IBattlable join)
    {
        if(joinTarget != null)
        {
            joinTarget.OnJoined -= JoinBattle;
        }

        joinTarget = join;
        joinTarget.OnJoined += JoinBattle;
    }

    /// @brief バトルから退場する対象を設定します
    public void SetLeaveTarget(IBattlable leave)
    { 
        if(leaveTarget != null)
        {
            leaveTarget.OnLeft -= LeaveBattle;
        }

        leaveTarget = leave;
        leaveTarget.OnLeft += LeaveBattle;
    }

    /// @brief 参加処理を行います
    public void JoinBattle(GameObject character)
    {
        joinCharacter.Add(character);
        Debug.Log("Character joined: " + character);
        if (character.CompareTag("Player"))
        { 
            SetState(BattleStateInput.START);
        }
    }

    /// @brief 退場処理を行います
    public void LeaveBattle(GameObject character)
    {
        if(!joinCharacter.Contains(character))
        {
            Debug.LogError("参加していないキャラクターを参照しています!!");
            return;
        }

        foreach(GameObject join in joinCharacter)
        {
            if(join != character)
            {
                continue;
            }

            joinCharacter.Remove(join);
            return;
        }
    }

    /// @brief 参加しているキャラクターを取得します
    public List<GameObject> GetJoinCharacter()
    {
        return joinCharacter;
    }
}
