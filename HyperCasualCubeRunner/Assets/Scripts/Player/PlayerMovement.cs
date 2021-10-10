using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Tweening library:
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    //  Movement:
    [Header("Movement")]
    [SerializeField] private bool shouldMove = true;
    [Tooltip("Movement speed of cube")]
    [SerializeField] private float moveForce = 800.0f;
    [SerializeField] private float moveSpeedClamp = 30.0f;

    [Header("Abilities")]
    [SerializeField] private bool isHyper = false;
    [SerializeField] private int currentCubeTypeID = 0;
    [SerializeField] private GameObject pfHyperStarted = null;
    [SerializeField] private GameObject pfHyperEnded = null;

    //  Lives:
    [Header("Player Lives")]
    [SerializeField] int livesMax = 3;
    [SerializeField] int livesCurrent = 3;
    [SerializeField] UIShapeCounter livesCounter = null;

    //  Lane switching:
    [Header("Lane Switching")]
    private LaneManager laneManager = null;
    [Tooltip("Current Lane.")]
    [SerializeField] private int currentLane = 1;
    [Tooltip("Duration for tweening to new lane.")]
    [SerializeField] private float switchDuration = 0.5f;
    [Tooltip("Ease Type for tweening.")]
    [SerializeField] private Ease switchAnimEase = Ease.InOutBack;

    //  Other components:
    [Space]
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private TrailRenderer trail = null;
    private Animator animator = null;
    private CubeExploder cubeExploder = null;

    //  Time and Camera controllers:
    private TimeController timeController = null;
    private CameraController camController = null;

    //  Other managers:
    private PowerupController powerController = null;
    private ScoreManager scoreManager = null;
    private CubeTypeManager cubeTypeManager = null;
    private PlayerSettings playerSettings = null;

    //  Delegates for specific events:
    public delegate void OnHyperActivated();
    public OnHyperActivated onHyperActivated;
    public delegate void OnHyperDeactivated();
    public OnHyperDeactivated onHyperDeactivated;
    public delegate void OnPlayerDeath();
    public OnPlayerDeath onPlayerDeath;

    private AudioManager audioManager = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cubeExploder = GetComponent<CubeExploder>();
        timeController = GameObject.FindObjectOfType<TimeController>();
        camController = GameObject.FindObjectOfType<CameraController>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        powerController = GetComponent<PowerupController>();
    }

    private void Start()
    {
        laneManager = LaneManager.Instance;
        if (laneManager == null)
            Debug.LogWarning(gameObject.name + ": LaneManager Instance not found.");

        cubeTypeManager = CubeTypeManager.Instance;
        if (cubeTypeManager == null)
            Debug.LogWarning(gameObject.name + ": CubeTypeManager Instance not found.");

        playerSettings = GameObject.FindObjectOfType<PlayerSettings>();

        //  Convert cube type to starting ID:
        if (playerSettings)
            currentCubeTypeID = playerSettings.GetPlayerStartingType();
        ConvertCubeType(cubeTypeManager.GetCubeTypeWithID(currentCubeTypeID));
        //  Update hearts UI:
        livesCounter.UpdateSectionCounter(livesCurrent);

        //  Audio:
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        //  InGame music:
        audioManager.PlayFromBank("InGameMusic", 3.0f, true);
    }


    // Update is called once per frame
    void Update()
    {
        if (!shouldMove)
            return;

        //  Check Input:
        CheckInput();
    }

    private void FixedUpdate()
    {
        if (!shouldMove)
            return;

        rb.AddForce(0, 0, moveForce * Time.deltaTime, ForceMode.Force);

        //  Clamp speed:
        rb.velocity = new Vector3(0, 0, Mathf.Clamp(rb.velocity.z, 0.0f, moveSpeedClamp));

    }

    public void OnHitObstacle(Obstacle obstacle)
    {
        //  If player is currently in Hyper mode, or matches obstacle cube type:
        if (isHyper || currentCubeTypeID == obstacle.GetCubeType())
        {
            //  Smash through obstacle:
            obstacle.ExplodeObstacle(rb.velocity);

            //  Points:
            scoreManager.AddPointsObstacleSmashed();

            //  Audio:
            if (audioManager)
                audioManager.PlaySound("HitObstacleLight");
        }
        //  Lose a life:
        else if (livesCurrent > 0)
        {         
            UpdateLife(-1);

            //  Audio:
            if (audioManager)
                audioManager.PlaySound("HitObstacleHeavy");

            //  Smash through obstacle if we still have lives:
            if (livesCurrent > 0)
                obstacle.ExplodeObstacle(rb.velocity);
        }

        //  Camera shake:
        camController.CameraShake();
    }

    public void OnHitConverter(CubeType newType)
    {
        if (newType != null)
        {
            //  ConvertCubeType:
            ConvertCubeType(newType);

            //  Slowmo:
            timeController.ActivateSlowMo();

            //  Points:
            scoreManager.AddPointsCollectedItem();

            //  Audio:
            if (audioManager)
                audioManager.PlaySound("HitConverter");
        }
        else
            Debug.LogWarning(gameObject.name + ": New Cube Type is null.");
    }

    private void ConvertCubeType(CubeType newType)
    {
        if (newType == null)
            return;
        //  Apply new properties (ID and material):
        currentCubeTypeID = newType.GetCubeID();
        meshRenderer.material = newType.GetCubeMaterial();

        //  Player style is emission texture on material:
        if(playerSettings)
            meshRenderer.material.SetTexture("_EmissionMap", playerSettings.GetPlayerSkin());

        //  Background colour:
        camController.SetBackgroundColour(newType.GetBackgroundMaterial());

        //  Trail:
        trail.material = newType.GetTrailMaterial();
    }

    public void UpdateLife(int amount)
    {
        //  Update:
        livesCurrent += amount;

        //  Update hearts UI:
        livesCounter.UpdateSectionCounter(livesCurrent);

        //  Check death:
        if (livesCurrent < 1)
            Die();

        //  Cap:
        if (livesCurrent > livesMax)
            livesCurrent = livesMax;
    }

    public void Die()
    {
        meshRenderer.gameObject.SetActive(false);
        Vector3 currentVel = rb.velocity * 0.4f;
        rb.velocity = Vector3.zero;
        cubeExploder.ExplodeCube(currentVel, meshRenderer.material);

        shouldMove = false;

        //  Slow mo:
        timeController.ActivateSlowMo();

        //  Audio:
        if (audioManager)
            audioManager.PlaySound("PlayerDeath");

        //  Invoke delegate:
        if (onPlayerDeath != null)
            onPlayerDeath.Invoke();
    }

    private void CheckInput()
    {
        //  Left:
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            UpdateLane(true);
        //  Right:
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            UpdateLane(false);
        //  Powerup:
        if (Input.GetKeyDown(KeyCode.Space))
            powerController.ActivateNextPowerup();
    }

    public void UpdateLane(bool left)
    {
        if (!shouldMove)
            return;

        if (left)
        {
            //  If we are not at the left end:
            if (currentLane > 0)
            {
                currentLane--;
                if (animator)
                    animator.SetTrigger("MoveLeft");
            }
        }
        else
        {
            //  If we are not at the right end:
            if (currentLane < laneManager.GetLastLaneID())
            {
                currentLane++;
                if (animator)
                    animator.SetTrigger("MoveRight");
            }
        }

        //  Audio:
        if (audioManager)
            audioManager.PlaySound("ChangeLane");

        //  Tween to chosen lane:
        rb.DOMoveX(laneManager.GetLanePosition(currentLane), switchDuration)
            .SetEase(switchAnimEase);
    }

    public void ActivateHyperBoost()
    {
        //  Set as hyper:
        isHyper = true;

        //  Camera:
        camController.ToFastCam();

        //  Effect:
        if (pfHyperStarted)
            Instantiate(pfHyperStarted, this.transform);

        //  Call delegate subscribed functions:
        if (onHyperActivated != null)
            onHyperActivated.Invoke();
    }

    public void DeactivateHyperBoost()
    {
        //  Deactivate hyper:
        isHyper = false;

        //  Camera:
        camController.ToDefaultCam();

        //  Effect:
        if (pfHyperStarted)
            Instantiate(pfHyperEnded, this.transform);

        //  Slow mo:
        timeController.ActivateSlowMo();

        //  Call delegate subscribed functions:
        if (onHyperDeactivated != null)
            onHyperDeactivated.Invoke();
    }

    public int GetCurrentCubeType() { return currentCubeTypeID; }
    public bool GetIsHyper() { return isHyper; }
    public Vector3 GetCurrentVelocity() { return rb.velocity; }
}
