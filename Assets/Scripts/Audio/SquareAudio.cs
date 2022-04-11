using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAudio : MonoBehaviour
{
    public AudioClip rotateLSound, rotateRSound, windSound,
        paintSound, errorSound, pickupSound, dropSound;

    public AudioClip[] placeSounds;

    public AudioClip[] matchSounds;

    private GameAudio gameAudio;

    public void Start()
    {
        gameAudio = GameObject.Find("Main Camera").GetComponent<GameAudio>();
    }

    public void PlayErrorSound()
    {
       
    }

}
