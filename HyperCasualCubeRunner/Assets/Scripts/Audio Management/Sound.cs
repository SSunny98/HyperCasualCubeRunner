using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Music,
    SFX
}

[System.Serializable]
public class Sound
{
    //  Name of the clip (used as ID for playing sound):
    public string sName;
    public AudioClip sClip;

    [Tooltip("Volume compared to other sounds.")]
    [Range(0.0f, 1.0f)]
    public float sVolume = 1.0f;

    private float currentOverallVolume = 1.0f;

    [Tooltip("Should this sound loop?")]
    public bool sLoop = false;

    [Tooltip("When Master Sound is UNmuted, should this continue to play? (e.g. for BGM and other constrant sounds)")]
    public bool playOnUnmute = false;

    [Tooltip("What type of sound is this? (Music/SFX)")]
    public SoundType soundType = SoundType.SFX;

    [HideInInspector]
    public bool shouldPlay = false;

    [HideInInspector]
    public AudioSource source;

    public Coroutine currentRoutine = null;

    public void UpdateOverallSoundVolume(SoundSettings soundSettings)
    {
        currentOverallVolume = sVolume;

        if (soundSettings != null)
        {
            currentOverallVolume *= soundSettings.GetMasterVolume();
            switch (soundType)
            {
                case SoundType.Music:
                    {
                        currentOverallVolume *= soundSettings.GetMusicVolume();
                    }
                    break;
                case SoundType.SFX:
                    {
                        currentOverallVolume *= soundSettings.GetSFXVolume();
                    }
                    break;
            }
        }

        source.volume = currentOverallVolume;
    }

    public void PlayTheSound()
    {
        source.Play();
    }

    public float GetCurrentOverallVolume()
    {
        return currentOverallVolume;
    }
}

//  Custom functions for single audio clips:
public static class AudioCustomFunctions
{
    //  Fade out a sound:
    public static IEnumerator FadeOut(Sound sound_, float fade_time_)
    {
        //  Get current source volume:
        float start_volume = sound_.source.volume;
        while (sound_.source.volume > 0f)
        {
            sound_.source.volume -= start_volume * Time.deltaTime / fade_time_;
            yield return null;
        }

        sound_.source.Stop();
        //  Reset source volume to 0:
        sound_.source.volume = 0.0f;

        //  Reset current couroutine:
        sound_.currentRoutine = null;
    }

    //  Fade in a sound:
    public static IEnumerator FadeIn(Sound sound_, float fade_time_, bool force_fade_ = false)
    {
        float sound_max = sound_.GetCurrentOverallVolume();
        float start_volume = sound_.source.volume;
        //  If should force fade, start from 0f:
        if (force_fade_)
            start_volume = 0f;

        sound_.source.volume = start_volume;

        while (sound_.source.volume < sound_max)
        {
            sound_.source.volume += sound_max * Time.deltaTime / fade_time_;
            yield return null;
        }
        sound_.source.volume = sound_max;

        //  Reset current couroutine:
        sound_.currentRoutine = null;

    }
}
