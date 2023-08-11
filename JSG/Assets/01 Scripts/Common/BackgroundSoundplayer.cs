using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundplayer : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundClip;

    private AudioSource backgroundSound;

    void Start()
    {
        backgroundSound = this.gameObject.AddComponent<AudioSource>();
        backgroundSound.playOnAwake = false;
        backgroundSound.loop = true;

        // 게임이 종료되었을 때, 배경음악을 멈춥니다.
        GameManager.Instance.Player.OnDeath += GameOver;
    }

    void GameStart(object sender, EventArgs args)
    {
        backgroundSound.Play();
    }

    void GameOver(object sender, EventArgs args)
    {
        backgroundSound.Stop();
    }
}
