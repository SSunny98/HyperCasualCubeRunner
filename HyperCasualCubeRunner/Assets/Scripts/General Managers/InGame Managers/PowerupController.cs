using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupController : MonoBehaviour
{
    [Space]
    [Tooltip("Currently collected Powers.")]
    [SerializeField] private List<Power> collectedPowers = new List<Power>();
    [Tooltip("Maximum number of powers that can be held at once.")]
    [SerializeField] private int maxNumPowers = 5;

    [Space]
    [SerializeField] private bool isPowerActive = false;
    private Power currentPower = null;
    [Tooltip("UIShapeCounter for powerups.")]
    [SerializeField] private UIShapeCounter powerupsCounter = null;
    [Tooltip("Progress bar used during the Hyper ability.")]
    [SerializeField] private UIProgressBar progressBarHyper = null;

    PlayerMovement player = null;
    ScoreManager scoreManager = null;

    AudioManager audioManager = null;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    private void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();

        powerupsCounter.UpdateSectionCounter(collectedPowers.Count);

        //  Subscribe to progress bar finished event:
        if (progressBarHyper)
            progressBarHyper.onTimerFinished += OnTimerFinished;
    }

    public void AddPower(Power newPower)
    {
        //  Max reached:
        if (collectedPowers.Count >= maxNumPowers)
            return;

        //  Check if power needs a timer:
        if (newPower.GetPowerNeedsTimer())
        {
            //  Add to list:
            collectedPowers.Add(newPower);

            //  Update UI:
            powerupsCounter.UpdateSectionCounter(collectedPowers.Count);

            //  Audio:
            if (audioManager)
                audioManager.PlaySound("PowerUpAdded");
        }
        //  Activate power straight away if it does NOT need a timer:
        else
            ActivatePower(newPower);

        //  Points:
        scoreManager.AddPointsCollectedItem();
    }

    public void ActivateNextPowerup()
    {
        //  If currently using, exit without activating another powerup:
        if (isPowerActive)
            return;

        //  Activate next powerup next in list:
        if (collectedPowers.Count > 0)
        {
            ActivatePower(collectedPowers[0]);

            //  Remove from list and update UI counter:
            collectedPowers.RemoveAt(0);
            powerupsCounter.UpdateSectionCounter(collectedPowers.Count);
        }
    }

    private void ActivatePower(Power power)
    {
        switch (power.GetPowerType())
        {
            default:
            case PowerType.HyperBoost:
                {
                    player.ActivateHyperBoost();
                    currentPower = power;
                    StartTimer(power.GetPowerValue());

                    //  Audio:
                    if (audioManager)
                        audioManager.PlaySound("HyperOn");
                }
                break;
            case PowerType.ExtraPoints:
                {
                    //  Add extra points:
                    scoreManager.AddPoints((int)power.GetPowerValue());

                    //  Audio:
                    if (audioManager)
                        audioManager.PlaySound("ExtraPoints");
                }
                break;
            case PowerType.ExtraLife:
                {
                    //  Add extra life:
                    player.UpdateLife((int)power.GetPowerValue());

                    //  Audio:
                    if (audioManager)
                        audioManager.PlaySound("ExtraLife");
                }
                break;
        }
    }

    private void DeactivatePower(Power power)
    {
        //  Reset ability:
        switch (power.GetPowerType())
        {
            default:
            case PowerType.HyperBoost:
                {
                    currentPower = null;
                    player.DeactivateHyperBoost();

                    //  Audio:
                    if (audioManager)
                        audioManager.PlaySound("HyperOff");
                }
                break;
        }

        isPowerActive = false;
    }

    //  Start timer:
    private void StartTimer(float newDuration)
    {
        isPowerActive = true;
        progressBarHyper.StartProgressBar(newDuration);
    }

    private void OnTimerFinished()
    {
        DeactivatePower(currentPower);
    }

    //  Get current number of collected (unused) powerups:
    public int GetCurrentPowerupsCount() { return collectedPowers.Count; }
}
