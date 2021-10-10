using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator = null;

    [SerializeField] private float transitionTime = 1.0f;

    bool currentlyLoading = false;

    public void LoadAScene(string sceneName)
    {
        if (!currentlyLoading)
            StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        currentlyLoading = true;
        //  Play animation:
        transitionAnimator.SetTrigger("SlideInRight");

        //  Wait:
        yield return new WaitForSeconds(transitionTime);

        //  Load scene:
        SceneManager.LoadScene(sceneName);

        //  Play animation:
        transitionAnimator.SetTrigger("SlideOutLeft");

        currentlyLoading = false;
    }
}
