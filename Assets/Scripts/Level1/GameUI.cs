using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timerText;
    public GameObject pausePanel;
    public Button pauseButton;
    public Button retryButton;
    public Button continueButton;
    public Button levelSelectButton;

    private int coinsCollected = 0;
    private float levelTimer = 120f; // Start at 120 seconds (2 minutes)
    private bool isPaused = false;

    void Start()
    {
        // Load previous coins from PlayerPrefs
        if (PlayerPrefs.HasKey("TotalCoins"))
        {
            coinsCollected = PlayerPrefs.GetInt("TotalCoins");
        }

        UpdateCoinUI();
        UpdateTimerUI();

        // Button Listeners
        pauseButton.onClick.AddListener(TogglePause);
        retryButton.onClick.AddListener(RetryLevel);
        continueButton.onClick.AddListener(ResumeGame);
        levelSelectButton.onClick.AddListener(GoToLevelSelection);
    }

    void Update()
    {
        if (!isPaused && levelTimer > 0)
        {
            levelTimer -= Time.deltaTime; // Countdown
            UpdateTimerUI();

            if (levelTimer <= 0)
            {
                levelTimer = 0;
                GameOver();
            }
        }
    }

    void UpdateCoinUI()
    {
        coinsText.text = "Coins: " + coinsCollected;
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(levelTimer / 60);
        int seconds = Mathf.FloorToInt(levelTimer % 60);
        timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateCoinUI();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToLevelSelection()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelection");
    }

    public int GetCoinsCollected() { return coinsCollected; }
    public float GetRemainingTime() { return levelTimer; }

    void GameOver()
    {
        UnityEngine.Debug.Log("Time is up! Game Over!");
        // You can show a Game Over screen or reload the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
