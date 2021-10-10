using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    //  Power up Manager:
    [Tooltip("All possible Powerups, PowerupSpawner Randomly chooses from this list.")]
    [SerializeField] private List<Power> allPowers = new List<Power>();

    //  Randomly choose a power:
    public Power GetRandomPower()
    {
        if (allPowers.Count < 1)
            return null;

        return allPowers[Random.Range(0, allPowers.Count)];
    }

}
