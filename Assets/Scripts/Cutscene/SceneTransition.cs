using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance;
    public Animator fadeAnimator;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensures SceneTransition persists across scenes
        }
        else if (instance != this)
        {
            UnityEngine.Debug.LogWarning("Duplicate SceneTransition detected. Destroying extra instance.");
            Destroy(gameObject); // Prevents multiple instances
            return;
        }

        if (fadeAnimator == null)
        {
            fadeAnimator = GetComponentInChildren<Animator>(true);
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (!isTransitioning)
        {
            StartCoroutine(FadeAndLoad(sceneName));
        }
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        isTransitioning = true;

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("End"); // Play Fade Out animation
            yield return new WaitForSeconds(fadeAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        SceneManager.LoadScene(sceneName);

        yield return null;

        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("Start"); // Play Fade In animation
        }

        isTransitioning = false;
    }
}
