using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class LevelSelectionManager : MonoBehaviour
{
    public Button[] levelButtons;
    public GameObject[] lockOverlays;
    private SceneTransition sceneTransition;

    void Start()
    {
        StartCoroutine(FindSceneTransition());

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > unlockedLevel)
            {
                levelButtons[i].interactable = false;
                lockOverlays[i].SetActive(true);
            }
            else
            {
                levelButtons[i].interactable = true;
                lockOverlays[i].SetActive(false);
            }
        }
    }

    IEnumerator FindSceneTransition()
    {
        // Wait a frame to allow objects to initialize
        yield return null;

        // Try to find SceneTransition normally
        sceneTransition = FindObjectOfType<SceneTransition>();

        // If not found, force search inside "FadeCanvas"
        if (sceneTransition == null)
        {
            GameObject fadeCanvas = GameObject.Find("FadeCanvas");
            if (fadeCanvas != null)
            {
                sceneTransition = fadeCanvas.GetComponent<SceneTransition>();
            }
        }

        // Final check and log an error if missing
        if (sceneTransition == null)
        {
            UnityEngine.Debug.LogError("SceneTransition object is still missing! Make sure FadeCanvas has SceneTransition.cs");
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (sceneTransition == null)
        {
            UnityEngine.Debug.LogError("SceneTransition object is missing! Scene cannot transition.");
            return;
        }

        if (levelIndex == 1)
        {
            sceneTransition.ChangeScene("Cutscene1");
        }
        else
        {
            sceneTransition.ChangeScene("Level" + levelIndex);
        }
    }

    public void BackToMainMenu()
    {
        if (sceneTransition == null)
        {
            UnityEngine.Debug.LogError("SceneTransition object is missing! Cannot go back to Main Menu.");
            return;
        }

        sceneTransition.ChangeScene("MainMenu");
    }
}
