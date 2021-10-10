using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    [SerializeField] private Image imgProgressFill = null;

    [SerializeField] private bool isTimerActive = false;
    [SerializeField] private float timerCurrent = 0.0f;
    [SerializeField] private float timerMax = 0.0f;

    Animator animator = null;

    //  Delegates for specific events:
    public delegate void OnTimerFinished();
    public OnTimerFinished onTimerFinished;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        //  Start deactivated:
        gameObject.SetActive(false);
    }


    private void Update()
    {
        if (!isTimerActive)
            return;

        //  Using unscaled delta time incase we're currently using a slowmotion power:
        timerCurrent += Time.unscaledDeltaTime;
        imgProgressFill.fillAmount = 1.0f - (timerCurrent / timerMax);
        if (timerCurrent >= timerMax)
            FinishedTimer();
    }

    public void StartProgressBar(float newMaxTime)
    {
        isTimerActive = true;
        timerMax = newMaxTime;

        imgProgressFill.fillAmount = 1.0f;

        //  Animation:
        if (animator)
            animator.SetTrigger("PopIn");

        gameObject.SetActive(true);
    }

    public void FinishedTimer()
    {
        isTimerActive = false;
        timerCurrent = 0.0f;

        //  Hide progress bar:
        if (animator)
            animator.SetTrigger("PopOut");

        //  Invoke delegate:
        if (onTimerFinished != null)
            onTimerFinished.Invoke();
    }
}
