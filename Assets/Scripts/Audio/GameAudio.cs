using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class GameAudio : MonoBehaviour
{
    public AudioClip mainTrack;

    public AudioClip buttonSound;


    public AudioSource mainTrackSource;

    public AudioSource effectSoundSource;

    private List<AudioSource> audioSources;

    private float transitionTimer, transitionTime;

    private bool transitioning;

    private float recencyTime = 0.2f;
    private float recencyTimer = 0f;

    public HashSet<string> clipNames;

    // Start is called before the first frame update
    void Start()
    {
        clipNames = new HashSet<string>();
        audioSources = new List<AudioSource>();

        Menu_UI.StartSceneTransition += StartSceneTransition;

        mainTrackSource.clip = mainTrack;

        mainTrackSource.loop = true;

        mainTrackSource.volume = 1.0f;

        if (Game.mode != Game.Mode.Edit)
        {
            mainTrackSource.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            transitionTimer -= Time.deltaTime;

            mainTrackSource.volume = transitionTimer / transitionTime;
            if(transitionTimer < 0)
            {
                transitioning = false;
            }
        }


        List<AudioSource> activeSources = new List<AudioSource>();
        //Clean up inactive audio sources
        foreach(AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                if (source.clip != null)
                {
                    clipNames.Remove(source.clip.name);
                }

                Destroy(source);

            }
            else
            {
                activeSources.Add(source);
            }
        }
        if(recencyTimer > 0f)
        {
            recencyTimer -= Time.deltaTime;
        }
        audioSources = activeSources;
    }


    private void _PlayWinSound(AudioClip clip)
    {
        PlayGameSound(clip);
        StartSceneTransition(0.8f);
        
    }

    public void PlayGameSound(AudioClip clip)
    {

        bool needNewSource = true;
        foreach(AudioSource source in audioSources)
        {

            if (!source.isPlaying)
            {
                source.clip = clip;
                source.Play();
                needNewSource = false;
            }
        }


        if (needNewSource && !clipNames.Contains(clip.name))
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = clip;
            newSource.Play();
            //Debug.Log("Playing " + clip.name);
            foreach(AudioSource a in audioSources)
            {
                a.volume = a.volume / 2f;
            }
            audioSources.Add(newSource);

           
        }
    }

    public void PlayButtonSound()
    {
        effectSoundSource.clip = buttonSound;
        effectSoundSource.Play();
    }

    public void StartSceneTransition(float timeToTransition)
    {
        transitioning = true;

        transitionTime = timeToTransition;
        transitionTimer = timeToTransition;

    }
}
