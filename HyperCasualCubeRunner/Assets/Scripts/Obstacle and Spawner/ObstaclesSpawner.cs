using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [SerializeField] ObstaclePooler pooler = null;

    [Tooltip("Z position difference between spawning obstacles.")]
    [SerializeField] private float distBetweenRows = 8.0f;
    [Tooltip("Current spawn position in Z axis.")]
    [SerializeField] private float currentSpawnPos = 8.0f;

    [Space]
    [SerializeField] private int initialSpawn = 20;

    //  Managers:
    private CubeTypeManager cubeTypeManager = null;
    private LaneManager laneManager = null;

    private float lastNotified = 0.0f;

    private void Awake()
    {

    }

    private void Start()
    {
        //  Get manager instance:
        cubeTypeManager = CubeTypeManager.Instance;
        if (cubeTypeManager == null)
            Debug.LogWarning(gameObject.name + ": CubeTypeManager Instance not found.");
        laneManager = LaneManager.Instance;
        if (laneManager == null)
            Debug.LogWarning(gameObject.name + ": LaneManager Instance not found.");

        //  Spawn first 10 rows:
        for (int i = 0; i < initialSpawn; i++)
        {
            SpawnRowOfObstacles();
        }
    }

    //  Spawn next row of obstacles after oldest row of obstacles have passed:
    public void OnPassedObstacle(float notifiedFromZ)
    {
        //  If being signaled from new row (new Z position):
        if (lastNotified < notifiedFromZ)
        {
            SpawnRowOfObstacles();
            lastNotified = notifiedFromZ;
        }
    }

    public void SpawnRowOfObstacles()
    {
        //  Choose random combination of obstacles for this row:
        Vector3Int combination = laneManager.GetRandomObstacleCombination();

        //  Check we have required number of obstacles available:
        int req = CheckNumObstaclesRequired(combination);
        bool available = pooler.CheckHasRequiredObstaclesAvailable(req);

        //  Return failed if we dont have enough available:
        if (!available)
        {
            //Debug.LogWarning("Not enough obstacles.");
            return;
        }

        //  For each lane (left, middle, right):
        for (int i = 0; i < 3; i++)
        {
            //  If this lane needs an obstacle:
            if (combination[i] == 1)
            {
                //  Spawn from pool (as a random CubeType):
                Vector3 obstPos = new Vector3(laneManager.GetLanePosition(i), 0.7f, currentSpawnPos);
                pooler.SpawnObstacle(obstPos, cubeTypeManager.GetRandomCubeType());
            }
        }

        //  Increment row:
        currentSpawnPos += distBetweenRows;
    }

    //  Return obstacles required (in a combination, 1 = obstacle, 0 = no obstacle):
    private int CheckNumObstaclesRequired(Vector3Int comb) { return comb.x + comb.y + comb.z; }
}
