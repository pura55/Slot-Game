using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// KeyInput
/// 
///　どのキーが押されているかのスクリプト
/// </summary>

public class KeyInput : MonoBehaviour
{
    [SerializeField] private NumberManager numberManager;



    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current == null)
            return;

        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Spaceキー");
        }

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            numberManager.AddNumber();
        }


        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            numberManager.AddGold();
        }
    }
}
