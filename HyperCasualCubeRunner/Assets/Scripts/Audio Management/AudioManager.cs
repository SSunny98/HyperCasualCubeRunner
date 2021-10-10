using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] singleSounds;
    public SoundBank[] soundBanks;

    private SoundSettings soundSettings = null;

    //  Awake (initialise sound attributes & sources)
    protected virtual void Awake()
    {
        //  Loop through sounds and add audio source to each sound:
        foreach (Sound s in singleSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.sClip;

            //  Apply the specified setting:
            s.source.volume = s.sVolume;
            s.source.loop = s.sLoop;

            //  Set some default settings:
            s.source.pitch = 1.0f;
            s.source.playOnAwake = false;
            s.source.dopplerLevel = 0f;
        }

        //  Loop through soundbanks, loop through each sound in that bank and add audio source to each sound:
        foreach (SoundBank sb in soundBanks)
        {
            foreach (Sound s in sb.differentSounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.sClip;

                //  Apply the specified setting:
                s.source.volume = s.sVolume;
                s.source.loop = s.sLoop;

                //  Set some default settings:
                s.source.pitch = 1.0f;
                s.source.playOnAwake = false;
                s.source.dopplerLevel = 0f;
            }
        }

    }

    protected virtual void Start()
    {
        soundSettings = GameManager.Instance?.GetSoundSettings();
    }

    //  Fade a sound in/out:
    protected void FadeSoundIn(Sound s, float fade_in_time_, bool should_force_fade_)
    {
        //  If a fade couroutine is currently running:
        if (s.currentRoutine != null)
            StopCoroutine(s.currentRoutine);
        s.currentRoutine = StartCoroutine(AudioCustomFunctions.FadeIn(s, fade_in_time_, should_force_fade_));
    }
    protected void FadeSoundOut(Sound s, float fade_out_time_)
    {
        //  If a fade couroutine is currently running:
        if (s.currentRoutine != null)
            StopCoroutine(s.currentRoutine);

        s.currentRoutine = StartCoroutine(AudioCustomFunctions.FadeOut(s, fade_out_time_));
    }


    //  Play specific sound:
    public void PlaySoundNoFade(string name_)
    {
        PlaySound(name_);
    }
    public virtual void PlaySound(string name_, float fade_in_time_ = -1.0f, bool should_force_fade_ = false)
    {
        Sound s = FindSoundSingle(name_);

        //  If sound is not found:
        if (s == null)
        {
            Debug.LogWarning("Sound: '" + name_ + "' not found!");
            return;
        }

        SetShouldPlay(s, true);
        UpdateAndPlay(s);

        //  If fade is specified:
        if (fade_in_time_ > 0.0f)
        {
            FadeSoundIn(s, fade_in_time_, should_force_fade_);
        }
    }
    protected Sound FindSoundSingle(string name_)
    {
        Sound s = Array.Find(singleSounds, sound => sound.sName == name_);
        return s;
    }


    //  Playing from bank functions:
    public void PlayFromBankNoFade(string name_)
    {
        PlayFromBank(name);
    }
    public virtual void PlayFromBank(string name_, float fade_in_time_ = -1.0f, bool should_force_fade_ = false)
    {
        //  Find bank:
        SoundBank sb = FindBank(name_);

        //  If soundbank is not found:
        if (sb == null)
        {
            Debug.LogWarning("Soundbank: '" + name_ + "' not found!");
            // exit function:
            return;
        }

        int index_to_play = 0;

        //  If the bank should be played sequentially:
        if (sb.shouldPlayInSequence)
        {
            index_to_play = NextIndexOfBank(sb);

            //  Play next sound:
            UpdateAndPlay(sb.differentSounds[index_to_play]);
        }
        //  Else If bank should be played randomly:
        else
        {
            //  Get random index:
            index_to_play = RandomIndexOfBank(sb);

            //  Play chosen sound;
            UpdateAndPlay(sb.differentSounds[index_to_play]);
        }

        //  If fade is specified:
        if (fade_in_time_ > 0.0f)
        {
            FadeSoundIn(sb.differentSounds[index_to_play], fade_in_time_, should_force_fade_);
        }
    }
    protected SoundBank FindBank(string name_)
    {
        // Lambda expression to find bank with the name:
        SoundBank sb = Array.Find(soundBanks, thisBank => thisBank.bankName == name_);
        return sb;
    }
    protected int NextIndexOfBank(SoundBank sb_)
    {
        //  Increment from last played index:
        int index_to_play = sb_.GetLastPlayedIndex() + 1;

        //  Loop back to first index if array size is exceeded:
        if (index_to_play >= sb_.differentSounds.Length)
            index_to_play = 0;

        //  Update last played index:
        sb_.SetLastPlayed(index_to_play);

        return index_to_play;
    }
    protected int RandomIndexOfBank(SoundBank sb_)
    {
        //  Get length of array:
        int num_of_sounds = sb_.differentSounds.Length;

        //  Choose random element from array:
        int randomElement = UnityEngine.Random.Range(0, num_of_sounds - 1);

        return randomElement;
    }

    protected void SetShouldPlay(string name_, bool should_play_)
    {
        Sound s = FindSoundSingle(name_);

        // If sound is not found:
        if (s == null)
        {
            Debug.LogWarning("Sound: '" + name_ + "' not found!");
            return;
        }

        s.shouldPlay = should_play_;
    }
    protected void SetShouldPlay(Sound s, bool should_play_)
    {
        s.shouldPlay = should_play_;
    }

    //  Stop sound functions:
    public void StopSound(string name_, float fade_out_time_ = -1f)
    {
        Sound s = FindSoundSingle(name_);

        // If sound is not found:
        if (s == null)
        {
            Debug.LogWarning("Sound: '" + name_ + "' not found!");
            return;
        }

        SetShouldPlay(s, false);

        //  If Fade not specified, stop sound isntantly:
        if (fade_out_time_ <= 0.0f)
            s.source.Stop();
        else
            FadeSoundOut(s, fade_out_time_);
    }
    public void StopSoundBank(string name_, float fade_out_time_ = -1f)
    {
        SoundBank sb = FindBank(name_);

        // If soundbank is not found:
        if (sb == null)
        {
            Debug.LogWarning("Soundbank: '" + name_ + "' not found!");
            return;
        }

        foreach (Sound s in sb.differentSounds)
        {
            if (s.source.isPlaying)
            {
                SetShouldPlay(s, false);

                //  If Fade not specified, stop sound isntantly:
                if (fade_out_time_ <= 0.0f)
                    s.source.Stop();
                else
                    FadeSoundOut(s, fade_out_time_);
            }
        }

    }
    public virtual void StopAllSounds()
    {
        // Loop through sounds in sound container:
        foreach (Sound s in singleSounds)
        {
            if (s.source.isPlaying)
            {
                if (s.playOnUnmute)
                {
                    //  Continue to play when sound gets turned on:
                    SetShouldPlay(s, true);
                }

                s.source.Stop();
            }
        }

        // Loop through soundbank and  stop each sound in the bank:
        foreach (SoundBank sb in soundBanks)
        {
            foreach (Sound s in sb.differentSounds)
            {
                if (s.source.isPlaying)
                {
                    if (s.playOnUnmute)
                    {
                        //  Continue to play when sound gets turned on:
                        SetShouldPlay(s, true);
                    }

                    s.source.Stop();
                }
            }
        }
    }

    //  Find all sounds that are currently playing and update volume:
    public void UpdateAllPlayingSoundsVolume(SoundSettings sound_settings_)
    {
        // Loop through sounds in sound container:
        foreach (Sound s in singleSounds)
        {
            //  If currently playing:
            if (s.source != null)
            {
                if (s.source.isPlaying)
                {
                    //  Update volume:
                    s.UpdateOverallSoundVolume(sound_settings_);
                }
            }
        }

        // Loop through soundbank and  stop each sound in the bank:
        foreach (SoundBank sb in soundBanks)
        {
            foreach (Sound s in sb.differentSounds)
            {
                if (s.source != null)
                {
                    //  If currently playing:
                    if (s.source.isPlaying)
                    {
                        //  Update volume:
                        s.UpdateOverallSoundVolume(sound_settings_);
                    }
                }
            }
        }
    }

    //  Find and play all sounds that are to be played when retoggling:
    protected void ReplayOnRetoggle()
    {
        //  Loop through sounds:
        foreach (Sound s in singleSounds)
        {
            //  If toggle was enabled:
            if (s.playOnUnmute)
            {
                //  Play:
                if (s.shouldPlay)
                    UpdateAndPlay(s);
            }
        }

        //  Loop through soundbanks:
        foreach (SoundBank sb in soundBanks)
        {
            foreach (Sound s in sb.differentSounds)
            {
                //  If toggle was enabled:
                if (s.playOnUnmute)
                {
                    //  Play:
                    if (s.shouldPlay)
                        UpdateAndPlay(s);
                }
            }
        }
    }

    protected void UpdateAndPlay(Sound s)
    {
        s.UpdateOverallSoundVolume(soundSettings);
        s.PlayTheSound();
    }

}
