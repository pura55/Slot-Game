using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    #region Config
    [SerializeField] private PlayerMovement playerMovement;
    #endregion

    #region State
    private PlayerState playerState = PlayerState.IDLE;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputMovement();
    }

    /// @brief “ü—Íˆ—‚ğs‚¢‚Ü‚·
    private void InputMovement()
    {
        if (Keyboard.current.dKey.isPressed && playerState == PlayerState.IDLE)
        {
            playerState = PlayerState.MOVE;
            playerMovement.SetSpeed(playerState);
        }
    }

    /// @brief €”õˆ—‚ğs‚¢‚Ü‚·
    public void ReadyBattle()
    {
        playerState = PlayerState.READY;
        playerMovement.SetSpeed(playerState);
    }
}
