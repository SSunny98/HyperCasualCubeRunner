using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    //  Style:
    [SerializeField] private Texture currentPlayerStyle = null;
    [SerializeField] private int currentPlayerStyleID = 0;

    //  Starting cube type:
    [SerializeField] private int currentPlayerStartingCubeType = 0;

    //  Save new style:
    public void SaveNewPlayerStyle(int newID, Texture newTexture)
    {
        currentPlayerStyleID = newID;
        currentPlayerStyle = newTexture;
    }

    //  Get currently saved Style:
    public Texture GetPlayerSkin() { return currentPlayerStyle; }
    public int GetPlayerSkinID() { return currentPlayerStyleID; }


    //  Save new starting type:
    public void SaveNewPlayerStartingType(int newType) { currentPlayerStartingCubeType = newType; }

    //  Get current starting type:
    public int GetPlayerStartingType() { return currentPlayerStartingCubeType; }


}
