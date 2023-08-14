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
        backgroundSound.clip = backgroundClip;

        // 게임 시작시 배경음악 재생
        GameManager.Instance.OnGameStarted += GameStart;
        // 게임 종료시 배경음악 정지
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
