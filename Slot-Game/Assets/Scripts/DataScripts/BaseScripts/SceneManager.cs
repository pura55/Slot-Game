using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private void Start()
    {
        // FukutoƒV[ƒ“‚ğ’Ç‰Á‚Å“Ç‚İ‚Ş
        SceneManager.LoadSceneAsync("Fukuto", LoadSceneMode.Additive);
    }
}

