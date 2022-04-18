using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAudio : MonoBehaviour
{
    public AudioClip rotateLSound, rotateRSound, windSound,
        paintSound, errorSound, goalSound, pickupSound, placeSound, dropSound;


    public AudioClip[] matchSounds;

    private GameAudio gameAudio;

    public void Start()
    {
        gameAudio = GameObject.Find("Main Camera").GetComponent<GameAudio>();
    }
    public void PlayRotateLSound()
    {
        gameAudio.PlayGameSound(rotateLSound);
    }

    public void PlayRotateRSound()
    {
        gameAudio.PlayGameSound(rotateRSound);
    }

    public void PlayWindSound()
    {
        gameAudio.PlayGameSound(windSound);
    }

    public void PlayPaintSound()
    {
        gameAudio.PlayGameSound(paintSound);
    }

    public void PlayErrorSound()
    {
        gameAudio.PlayGameSound(errorSound);
    }

    public void PlayPickupSound()
    {
        gameAudio.PlayGameSound(pickupSound);
    }


    public void PlayPlaceSound()
    {
        gameAudio.PlayGameSound(placeSound);
    }

    public void PlayDropSound()
    {
        gameAudio.PlayGameSound(dropSound);
    }


    public void PlayMatchSound()
    {
        int index = Random.Range(0, matchSounds.Length);

        gameAudio.PlayGameSound(matchSounds[index]);
    }

    public void PlayGoalSound()
    {
        gameAudio.PlayGameSound(goalSound);
    }
}
