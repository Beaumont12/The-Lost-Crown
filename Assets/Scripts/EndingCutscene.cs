using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class EndingCutscene : MonoBehaviour
{
    public TextMeshProUGUI storyText; // Reference to the TMP text component
    public float textSpeed = 0.05f; // Speed at which the text appears
    private string fullText;
    private bool textFinished = false; // Check if text is fully displayed

    void Start()
    {
        fullText = storyText.text; // Get the full text from TMP component
        storyText.text = ""; // Clear the text before starting
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        // Display the text one letter at a time
        foreach (char letter in fullText.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        textFinished = true; // Mark text as fully displayed

        // Wait for 2 seconds after text finishes before loading next scene
        yield return new WaitForSeconds(2f);
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        // Load the level selection screen after the text has finished
        SceneManager.LoadScene("LevelSelection"); // Change "LevelSelection" to your scene name for level selection
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Skip cutscene
        {
            StopAllCoroutines(); // Stop text animation immediately
            storyText.text = fullText; // Instantly display full text
            textFinished = true; // Mark text as finished

            // Skip directly to next scene if text is already finished
            if (textFinished)
            {
                LoadNextScene();
            }
        }
    }
}
