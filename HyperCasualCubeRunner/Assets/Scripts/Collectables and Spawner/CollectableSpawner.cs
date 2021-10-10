using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    private CollectablePooler pooler = null;

    [Space]
    [Tooltip("Spawning cooldown (min/max time till next spawn).")]
    [SerializeField] Vector2 timerRange = new Vector2(5.0f, 10.0f);
    [SerializeField] private bool isSpawnerActive = true;
    [Tooltip("Currently timer for spawning.")]
    [SerializeField] private float currentTimer = 0.0f;
    [Tooltip("Currently chosen spawn time.")]
    [SerializeField] private float nextSpawnTime = 0.0f;

    [Header("Positioning:")]
    [Tooltip("Position on Y axis.")]
    [SerializeField] private float spawnYHeight = 0.7f;
    [Tooltip("Distance ahead of player when spawning (Z axis).")]
    [SerializeField] private float spawnAheadDistance = 100.0f;

    //  Spawn checker:
    private SpawnChecker spawnChecker = null;

    //  Managers:
    private LaneManager laneManager = null;
    PlayerMovement player = null;

    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
        pooler = GetComponent<CollectablePooler>();
        spawnChecker = GetComponent<SpawnChecker>();
    }

    private void Start()
    {
        //  Get manager instance:
        laneManager = LaneManager.Instance;
        if (laneManager == null)
            Debug.LogWarning(gameObject.name + ": LaneManager Instance not found.");

        //  Subscribe to player death event:
        if (player != null)
            player.onPlayerDeath += OnPlayerDeath;

        //  First spawn:
        ResetForNextSpawn();
    }

    private void Update()
    {
        if (!isSpawnerActive)
            return;

        currentTimer += Time.deltaTime;
        if (currentTimer > nextSpawnTime)
            SpawnACollectable();
    }

    private void SpawnACollectable()
    {
        //  Spawn from pool:
        Vector3 newPos = new Vector3(laneManager.GetRandomLanePosition(), spawnYHeight, (int)(player.transform.position.z + spawnAheadDistance));

        //  Find a valid spawn point:
        //  Ensure it doesnt spawn inside obstacles etc.
        bool valid = false;
        while (!valid)
        {
            valid = spawnChecker.CheckSpawnPointValid(newPos);

            //  Update position:
            if (!valid)
                newPos.z += 2.0f;
        }
        
        pooler.SpawnCollectable(newPos);

        //  Reset:
        ResetForNextSpawn();
    }

    private void ResetForNextSpawn()
    {
        //  Choose next spawn time:
        nextSpawnTime = Random.Range(timerRange.x, timerRange.y);
        //  Reset current timer:
        currentTimer = 0.0f;
    }

    //  Deactivate spawner on player death:
    private void OnPlayerDeath()
    {
        isSpawnerActive = false;
    }
}
