using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [field: SerializeField] public Transform GunFlareTransform { get; private set; }
    [field: SerializeField] public GameObject GunLight { get; private set; }

    void Start()
    {
        this.OnDeath += Death;

        GameManager.Instance.OnGameStarted += PlayerReset;
    }

    // 플레이어가 죽은 경우
    private void Death(object sender, EventArgs args)
    {
        GameManager.Instance.IsPlaying = false;
    }

    private void PlayerReset(object sender, EventArgs args)
    {
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        this.CurrentHp = MaxHp;
    }
}
