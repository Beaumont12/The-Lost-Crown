using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad = "Level2"; // Change for next level

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameUI gameUI = UnityEngine.Object.FindFirstObjectByType<GameUI>();

            // Save total coins collected across levels
            int totalCoins = gameUI.GetCoinsCollected();
            PlayerPrefs.SetInt("TotalCoins", totalCoins);

            // Save Level completion time (you can change the logic based on the level)
            PlayerPrefs.SetFloat("Time_Level1", gameUI.GetRemainingTime());

            // Unlock next level based on the level index
            int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
            int nextLevelIndex = int.Parse(sceneToLoad.Replace("Level", ""));
            if (nextLevelIndex > currentUnlocked)
            {
                PlayerPrefs.SetInt("UnlockedLevel", nextLevelIndex);
                PlayerPrefs.Save();
            }

            Time.timeScale = 1;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
