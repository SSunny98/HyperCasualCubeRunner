using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePooler : MonoBehaviour
{
    [Tooltip("Prefab for Collectable Item.")]
    [SerializeField] private GameObject pfCollectable = null;
    [Tooltip("Number of Collectables to spawn in pool.")]
    [SerializeField] private int poolSize = 10;

    //  Hold the Collectables in a list:
    private List<Collectable> pool = new List<Collectable>();

    private void Awake()
    {
        //  Initialise pool:
        InitPool();
    }

    private void InitPool()
    {
        if (pfCollectable == null)
        {
            Debug.LogWarning(gameObject.name + ": Collectable Prefab not referenced.");
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject newGO = Instantiate(pfCollectable, transform);

            //  Store reference to Collectable in list (start as deactivated):
            Collectable newCollectable = newGO.GetComponent<Collectable>();
            newCollectable.DeactivateCollectable();
            pool.Add(newCollectable);
        }
    }

    //  Find and spawn an available Collectable from the pool:
    public void SpawnCollectable(Vector3 newPos)
    {
        Collectable availableCollectable = FindAvailableCollectable();

        if (availableCollectable != null)
        {
            availableCollectable.transform.position = newPos;
            availableCollectable.transform.rotation = Quaternion.identity;
            availableCollectable.OnSpawned();
        }
    }

    //  Find an available Collectable from the pool:
    private Collectable FindAvailableCollectable()
    {
        for (int i = 0; i < poolSize; i++)
        {
            //  If this Collectable ready to spawn:
            if (pool[i].GetIsReadyToSpawn())
                return pool[i];
        }

        //  Not found an unused Collectable:
        Debug.LogWarning(gameObject.name + ": All Collectables in pool are being used!");
        return null;
    }
}
