using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoundplayer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float tempFloat = 0f;
    private AudioSource audio;
    private AudioClip hitClip;
    private AudioClip deathClip;

    private void Awake()
    {
        audio.playOnAwake = false;
    }


    private void PlayHotAudio()
    {
        audio.clip = hitClip;
        audio.Play();
    }

    private void PlayDeayhAudio()
    {
        audio.clip = deathClip;
        audio.Play();
    }
}
