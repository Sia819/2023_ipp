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

        GameManager.Instance.OnGameResetted += OnResetted;
    }

    // �÷��̾ ���� ���
    private void Death(object sender, EventArgs args)
    {
        GameManager.Instance.IsPlaying = false;
    }

    private void OnResetted()
    {
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        this.CurrentHp = MaxHp;
    }
}
