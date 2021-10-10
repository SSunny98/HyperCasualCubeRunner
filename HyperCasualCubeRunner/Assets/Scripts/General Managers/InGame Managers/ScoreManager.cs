using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    [Header("Scoring:")]
    [Tooltip("Points for smashing through obstacles.")]
    [SerializeField] private int scorePerObstacleSmash = 100;
    [Tooltip("Points for collecting objects.")]
    [SerializeField] private int scorePerItemCollected = 50;
    [Tooltip("Points for unused Powerups (calculated in GameOver).")]
    [SerializeField] private int scorePerUnusedPowerup= 100;

    [Tooltip("Current score")]
    [SerializeField] private int score = 0;

    [Header("UI:")]
    [SerializeField] TextMeshProUGUI txtDistance = null;
    [SerializeField] UICounter scoreCounter = null;

    PlayerMovement player = null;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Player distance:
        UpdateDistanceText();
    }

    //  Update distance text:
    private void UpdateDistanceText()
    {
        if (txtDistance == null)
            return;

        int currentZ = (int) player.transform.position.z;
        txtDistance.SetText(currentZ.ToString() + "m");
    }

    public void AddPoints(int amount)
    {
        score += amount;

        scoreCounter.CountTo(score);
    }

    public void AddPointsObstacleSmashed() { AddPoints(scorePerObstacleSmash); }
    public void AddPointsCollectedItem() { AddPoints(scorePerItemCollected); }


    public int GetCurrentScore() { return score; }
    public int GetScorePerUnusedPowerup() { return scorePerUnusedPowerup; }

}
