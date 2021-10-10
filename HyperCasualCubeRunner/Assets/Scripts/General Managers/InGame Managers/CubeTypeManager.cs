using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class for a single CubeType:
[System.Serializable]
public class CubeType
{
    [SerializeField] private string cubeName = "TYPE0";
    [SerializeField] private int cubeID = 0;
    [SerializeField] private Material cubeMaterial = null;
    [SerializeField] private Material bgMaterial = null;
    [SerializeField] private Material trailMaterial = null;

    //  Getters:
    public int GetCubeID() { return cubeID; }
    public Material GetCubeMaterial() { return cubeMaterial; }
    public Material GetBackgroundMaterial() { return bgMaterial; }
    public Material GetTrailMaterial() { return trailMaterial; }

}

//  Main manager class:
public class CubeTypeManager : MonoBehaviour
{
    //  Singleton manager:
    private static CubeTypeManager _instance;
    public static CubeTypeManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning(gameObject.name + ": Instance already exists, destroying this obj: " + this.gameObject.name);
            DestroyImmediate(this.gameObject);
        }
        else
            _instance = this;
    }

    //  All possible cube types:
    [Tooltip("List of possible CubeTypes")]
    [SerializeField] private List<CubeType> allCubeTypes = new List<CubeType>();

    [Space]
    [Tooltip("Material for obstacles when player is in hyper mode.")]
    [SerializeField] private Material hyperMaterial = null;
    [Tooltip("Material for background skybox when player is in hyper mode.")]
    [SerializeField] private Material hyperBGMaterial = null;

    //  Get a random cube type from the list:
    public CubeType GetRandomCubeType()
    {
        if (allCubeTypes.Count < 1)
            return null;
        return allCubeTypes[Random.Range(0, allCubeTypes.Count)];
    }
    //  Get a random cube type that is not the specified ID:
    public CubeType GetRandomCubeTypeExclude(int excludeID)
    {
        if (allCubeTypes.Count < 1)
            return null;

        CubeType chosen = allCubeTypes[Random.Range(0, allCubeTypes.Count)];
        while (chosen.GetCubeID() == excludeID)
        {
            chosen = allCubeTypes[Random.Range(0, allCubeTypes.Count)];
        }
        return chosen;
    }

    //  Get specific cube type with matching ID:
    public CubeType GetCubeTypeWithID(int withID)
    {
        foreach (CubeType cubeType in allCubeTypes)
        {
            if (cubeType.GetCubeID() == withID)
                return cubeType;
        }
        return null;
    }

    public Material GetHyperMaterial() { return hyperMaterial; }
    public Material GetHyperBGMaterial() { return hyperBGMaterial; }

    public int GetTotalTypes() { return allCubeTypes.Count; }
}
