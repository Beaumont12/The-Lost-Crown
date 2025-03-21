using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        // Change "LevelSelection" to "Level1_PixelPlains" if you want to go straight to Level 1
        FindFirstObjectByType<SceneTransition>().ChangeScene("LevelSelection");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); // Only works in a built game
    }
}
