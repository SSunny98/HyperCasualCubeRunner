using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundBank
{
    public string bankName = "BANKNAME";
    public Sound[] differentSounds;

    // Play the sounds is sequence:
    public bool shouldPlayInSequence = false;
    // If playing in sequence, track the last played sound index:
    private int lastPlayedIndex = -1;

    // Return last played index:
    public int GetLastPlayedIndex()
    {
        return lastPlayedIndex;
    }

    // Set last played index:
    public void SetLastPlayed(int new_val_)
    {
        lastPlayedIndex = new_val_;
    }

}
