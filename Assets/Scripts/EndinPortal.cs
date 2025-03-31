using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingPortal : MonoBehaviour
{
    public string sceneToLoad = "EndingCutscene"; // The scene where the ending cutscene happens

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the Slime is dead before allowing the player to proceed
            bool isSlimeDead = CheckSlimeDeath();

            if (isSlimeDead)
            {
                GameUI gameUI = UnityEngine.Object.FindFirstObjectByType<GameUI>();

                // Save total coins collected across levels
                int totalCoins = gameUI.GetCoinsCollected();
                PlayerPrefs.SetInt("TotalCoins", totalCoins);

                // Save Level completion time
                PlayerPrefs.SetFloat("Time_Level1", gameUI.GetRemainingTime());

                // Unlock next level if needed (optional, if there's another level after the cutscene)
                int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
                int nextLevelIndex = 2; // Update if necessary for your next level
                if (nextLevelIndex > currentUnlocked)
                {
                    PlayerPrefs.SetInt("UnlockedLevel", nextLevelIndex);
                    PlayerPrefs.Save();
                }

                Time.timeScale = 1;
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.Log("Defeat the Slime to proceed to the ending.");
                // You can display a message here or handle the player’s inability to proceed
            }
        }
    }

    bool CheckSlimeDeath()
    {
        GameObject slime = GameObject.FindGameObjectWithTag("Enemy"); // Ensure the Slime is tagged correctly
        if (slime != null)
        {
            SlimeAI slimeAI = slime.GetComponent<SlimeAI>(); // Ensure SlimeAI is attached to the Slime
            if (slimeAI != null)
            {
                return slimeAI.isDead; // Check if the Slime is dead
            }
        }
        return false; // Return false if Slime is not found
    }
}
