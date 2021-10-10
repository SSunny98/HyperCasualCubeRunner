using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private PlayerMovement player = null;
    private ScoreManager scoreManager = null;
    private PowerupController powerupController = null;

    [SerializeField] private Animator animatorGameOverPanel = null;

    [Space]
    [Tooltip("Delay before animating score texts (wait for panel 'PopIn' animation to finish).")]
    [SerializeField] private float delayBeforeScoreAnimation = 0.5f;

    [Header("Score UICounters:")]
    [SerializeField] private UICounter txtScore = null;
    [SerializeField] private UICounter txtDist = null;
    [SerializeField] private UICounter txtUnusedPowersNum = null;
    [SerializeField] private UICounter txtUnusedPowersScore = null;
    [SerializeField] private UICounter txtFinalScore = null;

    GameManager gameManager = null;
    AudioManager audioManager = null;

    private void Awake()
    {
        //  Find components:
        player = GameObject.FindObjectOfType<PlayerMovement>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        powerupController = GameObject.FindObjectOfType<PowerupController>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
            Debug.LogWarning(gameObject.name + ": GameManager Instance not found.");

        //  Subcribe to player death event:
        if (player)
            player.onPlayerDeath += PlayerDied;
    }

    private void PlayerDied()
    {
        //  Animate:
        animatorGameOverPanel.SetTrigger("PopIn");

        //  Invoke after specified delay:
        if (delayBeforeScoreAnimation > 0.0f)
            Invoke("AnimateScore", delayBeforeScoreAnimation);
        else
            AnimateScore();
    }

    private void AnimateScore()
    {
        //  Get scores:
        int mainScore = scoreManager.GetCurrentScore();
        int unusedPowers = powerupController.GetCurrentPowerupsCount();
        int unusedPowersScore = (int)(unusedPowers * scoreManager.GetScorePerUnusedPowerup());
        int finalScore = mainScore + unusedPowersScore;

        //  Animate all counters:
        txtScore.CountTo(mainScore);
        txtDist.CountTo((int)player.transform.position.z);
        txtUnusedPowersNum.CountTo(unusedPowers);
        txtUnusedPowersScore.CountTo(unusedPowersScore);
        txtFinalScore.CountTo(finalScore);
    }


    //  Called from buttons in gameover scene:
    public void GoToMainMenu()
    {
        if (gameManager)
            gameManager.ToMainMenu();
        else
            Debug.LogWarning(gameObject.name + ": GameManager is NULL.");

        //  Fade music:
        FadeOutGameMusic();
    }
    public void RestartCurrentScene()
    {
        if (gameManager)
            gameManager.RestartCurrentScene();
        else
            Debug.LogWarning(gameObject.name + ": GameManager is NULL.");

        //  Fade music:
        FadeOutGameMusic();
    }

    public void FadeOutGameMusic()
    {
        if (audioManager)
            audioManager.StopSoundBank("InGameMusic", 1.0f);
    }
}
