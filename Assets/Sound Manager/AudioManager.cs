using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{

    [Range(0f, 1f)]
    public float SoundVolume = 1f;

    public Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }
    private void OnEnable()
    {
        SoundVol();
    }
    public void SoundUIVolumeChange(float Volume)
    {
        SoundVolume = Volume;
        GetComponent<AudioSource>().volume = SoundVolume;
    }
    /// Calling of Below Method as ''' =>   GameObject.Find("AudioManager").SendMessage("play", "Click");

    public void play(string name)
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
            return;
        SoundVol();
        s.source.Play();
    }

    void SoundVol()
    {
        Sound s = Array.Find(sounds, Sound => Sound.name == name);
        if (s == null)
            return;
        s.source.volume = SoundVolume;
    }
}
