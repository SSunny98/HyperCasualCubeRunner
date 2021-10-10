using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooler : MonoBehaviour
{
    [Tooltip("Prefab for obstacle.")]
    [SerializeField] private GameObject pfObstacle = null;
    [Tooltip("Number of obstacles to spawn in pool.")]
    [SerializeField] private int poolSize = 50;

    //  Hold the projectiles in a list:
    private List<Obstacle> pool = new List<Obstacle>();

    [SerializeField] private ObstaclesSpawner spawner = null;

    private void Awake()
    {
        //  Initialise pool:
        InitPool();
    }

    private void InitPool()
    {
        if (pfObstacle == null)
        {
            Debug.LogWarning(gameObject.name + ": Prefab not referenced.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newGO = Instantiate(pfObstacle, transform);

            //  Store reference to obstacle in list:
            Obstacle newObstacle= newGO.GetComponent<Obstacle>();
            newObstacle.SetSpawnerRef(spawner);
            newObstacle.DeactivateObstacle();
            pool.Add(newObstacle);
        }
    }

    //  Find and spawn an available obstacle from the pool:
    public void SpawnObstacle(Vector3 newPos, CubeType newCubeType)
    {
        Obstacle availableObstacle = FindAvailableObstacle();

        if (availableObstacle != null)
        {
            availableObstacle.transform.position = newPos;
            availableObstacle.transform.rotation = Quaternion.identity;
            availableObstacle.ActivateObstacle(newCubeType);
        }
    }

    //  Find an available obstacle from the pool:
    private Obstacle FindAvailableObstacle()
    {
        //  For each obstacle in the pool:
        for (int i = 0; i < poolSize; i++)
        {
            //  Check availability of this obstacle:
            if (GetIsObstacleAvailable(i))
                return pool[i];
        }

        //  Not found an unused obstacle:
        Debug.LogWarning(gameObject.name + ": All Obstacles are being used!");
        return null;
    }

    //  Check that there are atleast the required amount of available obstacles in the pool:
    public bool CheckHasRequiredObstaclesAvailable(int required)
    {
        int currentAvailable = 0;
        //  For each obstacle:
        for (int i = 0; i < poolSize; i++)
        {
            //  Check this is available and increment counter:
            if (GetIsObstacleAvailable(i))
                currentAvailable++;

            //  If required amount is reached, return:
            if (currentAvailable >= required)
                return true;
        }

        return false;
    }

    //  Obstacle is available if it is NOT currently active or exploding:
    private bool GetIsObstacleAvailable(int i)
    {
        return (!pool[i].GetIsObstacleActive() && !pool[i].GetIsExploding());
    }
}
