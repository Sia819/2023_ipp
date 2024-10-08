using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이 환경에 따른  배경음악을 재생하거나 정지합니다.
/// </summary>
public class BackgroundSoundplayer : MonoBehaviour
{
    /// <summary> 배경음악 소리 파일 </summary>
    [SerializeField] private AudioClip backgroundClip;

    private AudioSource backgroundSound;

    #region Inspector Warning
    private void OnValidate()
    {
        if (backgroundClip == null) Debug.LogWarning($"{GetType().Name}컴포넌트의 {nameof(backgroundClip)}요소는 필수이므로 비어있을 수 없습니다.", this);
    }
    #endregion

    private void Awake()
    {
        // 게임 (시작&종료 시) 배경음악 (재생&정지)
        GameManager.Instance.OnGameStarted += GameStart;
        GameManager.Instance.Player.OnDeath += GameOver;

        backgroundSound = this.gameObject.AddComponent<AudioSource>();
        backgroundSound.playOnAwake = false;
        backgroundSound.loop = true;
        backgroundSound.clip = backgroundClip;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStarted -= GameStart;
        GameManager.Instance.Player.OnDeath -= GameOver;
    }

    /// <summary> 게임이 시작될 때 배경음악을 재생합니다. </summary>
    private void GameStart(object sender, EventArgs args)
    {
        backgroundSound.Play();
    }

    /// <summary>
    /// 게임 오버될 때 배경음악을 정지합니다. </summary>
    private void GameOver(object sender, EventArgs args)
    {
        backgroundSound.Stop();
    }
}
