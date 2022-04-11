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

    private float transitionTimer, transitionTime;

    private bool transitioning;


    // Start is called before the first frame update
    void Start()
    {
        Menu_UI.StartSceneTransition += StartSceneTransition;

        mainTrackSource.clip = mainTrack;

        mainTrackSource.loop = true;

        mainTrackSource.volume = 1.0f;

        mainTrackSource.Play();
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


    }


    private void _PlayWinSound(AudioClip clip)
    {
        PlayGameSound(clip);
        StartSceneTransition(0.8f);
        
    }

    public void PlayGameSound(AudioClip clip)
    {

        effectSoundSource.clip = clip;
        effectSoundSource.Play();
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
