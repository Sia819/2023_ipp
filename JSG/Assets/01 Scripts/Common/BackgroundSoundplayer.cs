using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundplayer : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundClip;

    private AudioSource backgroundSound;

    void Awake()
    {
        // 게임 (시작&종료 시) 배경음악 (재생&정지)
        GameManager.Instance.OnGameStarted += GameStart;
        GameManager.Instance.Player.OnDeath += GameOver;

        backgroundSound = this.gameObject.AddComponent<AudioSource>();
        backgroundSound.playOnAwake = false;
        backgroundSound.loop = true;
        backgroundSound.clip = backgroundClip;
    }

    private void GameStart(object sender, EventArgs args)
    {
        backgroundSound.Play();
    }

    private void GameOver(object sender, EventArgs args)
    {
        backgroundSound.Stop();
    }
}
