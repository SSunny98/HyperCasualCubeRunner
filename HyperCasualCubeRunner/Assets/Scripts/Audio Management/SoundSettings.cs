using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSettings
{
    //  Main volume slider:
    [Tooltip("The master volume affecting all sounds.")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float masterVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float musicVolume = 1.0f;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float sfxVolume = 1.0f;

    //  Get main volume:
    public float GetMasterVolume()
    {
        return masterVolume;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    //  Setters:
    public void SetMasterVolume(float new_value_)
    {
        masterVolume = new_value_;
        masterVolume = Mathf.Clamp(masterVolume, 0f, 1f);
    }
    public void SetMusicVolume(float new_value_)
    {
        musicVolume = new_value_;
        musicVolume = Mathf.Clamp(musicVolume, 0f, 1f);
    }
    public void SetSFXVolume(float new_value_)
    {
        sfxVolume = new_value_;
        sfxVolume = Mathf.Clamp(sfxVolume, 0f, 1f);
    }
    public void ChangeMasterVolume(float change_value_)
    {
        masterVolume += change_value_;
        masterVolume = Mathf.Clamp(masterVolume, 0f, 1f);
    }
    public void ChangeMusicVolume(float change_value_)
    {
        musicVolume += change_value_;
        musicVolume = Mathf.Clamp(musicVolume, 0f, 1f);
    }
    public void ChangeSFXVolume(float change_value_)
    {
        sfxVolume += change_value_;
        sfxVolume = Mathf.Clamp(sfxVolume, 0f, 1f);
    }
}
