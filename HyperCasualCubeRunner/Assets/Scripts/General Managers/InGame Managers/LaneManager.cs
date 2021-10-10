using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaneManager : MonoBehaviour
{
    //  Singleton manager:
    private static LaneManager _instance;
    public static LaneManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
            DestroyImmediate(this.gameObject);
        else
            _instance = this;
    }


    [Tooltip("X positions for each lane.")]
    [SerializeField] private List<float> allLanes = new List<float>{ -1.5f, 0.0f, 1.5f };

    [Tooltip("Different combinations of possible obstacle arrangements.(0 = no obstacle, 1 = obstacle)")]
    [SerializeField] private List<Vector3Int> obstacleCombinations = new List<Vector3Int>();


    //  Get X position for a specific lane: 
    public float GetLanePosition(int laneID) { return allLanes[laneID]; }

    //  Get last lane:
    public float GetLastLaneID() { return allLanes.Count - 1; }
    
    //  Choose a random lane position:
    public float GetRandomLanePosition()
    {
        if (allLanes.Count < 0)
            return 0.0f;

        return allLanes[Random.Range(0, allLanes.Count)];
    }

    //  Find a random combination of obstacle for a single row:
    public Vector3Int GetRandomObstacleCombination()
    {
        return obstacleCombinations[Random.Range(0, obstacleCombinations.Count)];
    }

}
