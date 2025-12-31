using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                s.source.loop = s.loop;
            }

        Play("BgMusic"); //start the background music
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found");
            return;
        }
        if (s.source == null)
        {
            Debug.Log("Sound " + name + "'s AudioSource is null");
            return;
        }
        if (s.source.loop)
        {
            s.source.Play();
        }
        else s.source.PlayOneShot(s.clip, s.volume);
    }

    public void Play(string name, float minPitch, float maxPitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found");
            return;
        }
        if (s.source == null)
        {
            Debug.Log("Sound " + name + "'s AudioSource is null");
            return;
        }
        s.source.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        if (s.source.loop)
        {
            s.source.Play();
        }
        else s.source.PlayOneShot(s.clip, s.volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
