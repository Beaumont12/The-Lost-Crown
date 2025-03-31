using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required to load scenes

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; // Reference to the TextMeshProUGUI component
    public GameObject backButton; // Reference to the Back Button GameObject

    void Start()
    {
        // Get the highest coins from PlayerPrefs
        int highestCoins = PlayerPrefs.GetInt("HighestCoins", 0);

        // Display the highest coins in the TextMeshPro component
        highScoreText.text = "Most Coins: " + highestCoins.ToString();

        // If you want to display the button immediately or make it visible only after the text is shown
        backButton.SetActive(true); // Make sure this button is active
    }

    // This function will be called when the player clicks the "Back to Level Selection" button
    public void GoBackToLevelSelection()
    {
        // Load the Level Selection scene
        SceneManager.LoadScene("LevelSelection"); // Make sure "LevelSelection" is the correct scene name
    }
}
