using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad = "Level2"; // Change for next level

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUI gameUI = FindObjectOfType<GameUI>();

            // Save total coins collected across levels
            int totalCoins = gameUI.GetCoinsCollected();
            PlayerPrefs.SetInt("TotalCoins", totalCoins);

            // Save Level 1 completion time
            PlayerPrefs.SetFloat("Time_Level1", gameUI.GetRemainingTime());

            // Reset time scale before loading the next level
            Time.timeScale = 1;

            // Load next scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
