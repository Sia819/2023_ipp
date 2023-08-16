using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : Entity
{
    [field: SerializeField] public Transform GunFlareTransform { get; private set; }
    [field: SerializeField] public GameObject GunLight { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        this.OnDeath += Death;
        GameManager.Instance.OnGameStarted += PlayerReset;
    }

    // 플레이어가 죽은 경우
    private void Death(object sender, EventArgs args)
    {
        GameManager.Instance.GameSet();
    }

    private void PlayerReset(object sender, EventArgs args)
    {
        this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 180f, 0f));
        this.CurrentHp = MaxHp;
    }
}
