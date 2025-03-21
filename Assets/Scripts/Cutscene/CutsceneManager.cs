using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public float textSpeed = 0.05f;
    private string fullText;
    private bool textFinished = false; // Check if text is fully displayed

    void Start()
    {
        fullText = storyText.text; // Get text from TMP component
        storyText.text = ""; // Clear before typing
        StartCoroutine(DisplayText());
    }

    IEnumerator DisplayText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        textFinished = true; // Mark text as fully displayed

        // Wait 2 seconds after text finishes before loading next scene
        yield return new WaitForSeconds(2f);
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        FindFirstObjectByType<SceneTransition>().ChangeScene("Level1");
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
