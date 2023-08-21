using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어로 정의 될 오브젝트 대상입니다.
/// </summary>
public sealed class Player : Entity
{
    [field: SerializeField] public Transform GunFlareTransform { get; private set; }
    [field: SerializeField] public GameObject GunLight { get; private set; }

    #region Inspector Warning
    private void OnValidate()
    {
        Validate.NullCheck(this, nameof(GunFlareTransform));
        Validate.NullCheck(this, nameof(GunLight));
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        this.OnDeath += Death;
        GameManager.Instance.OnGameStarted += PlayerReset;
    }

    private void OnDestroy()
    {
        this.OnDeath -= Death;
        GameManager.Instance.OnGameStarted -= PlayerReset;
    }

    /// <summary> 플레이어가 죽은 경우, 게임이 중단되도록 합니다. </summary>
    private void Death(object sender, EventArgs args)
    {
        GameManager.Instance.GameSet();
    }

    /// <summary> 게임이 리셋될 경우, 플레이어가 초기화될 것을 설정합니다. </summary>
    private void PlayerReset(object sender, EventArgs args)
    {
        this.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(0f, 180f, 0f));
        this.CurrentHp = MaxHp;
    }
}
