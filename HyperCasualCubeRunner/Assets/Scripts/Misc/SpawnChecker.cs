using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChecker : MonoBehaviour
{
    [SerializeField] private float checkRadius = 0.7f;
    [SerializeField] private LayerMask checkLayers = 0;
    [SerializeField] private List<string> tagsToCheck = new List<string>();

    public bool CheckSpawnPointValid(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, checkRadius, checkLayers);

        foreach (Collider collider in colliders)
        {
            for (int i = 0; i < tagsToCheck.Count; i++)
            {
                if (collider.CompareTag(tagsToCheck[i]))
                    return false;
            }
        }

        //  Not returned yet so no tags were hit:
        //  Return true, indicates spawn point is valid.
        return true;

    }

}
