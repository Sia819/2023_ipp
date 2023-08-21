using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Entity class오브젝트의 애니메이션을 제어합니다.
/// 특히 Move, Stop, Death, Attack 상태의 애니메이션을 제어합니다.
/// </summary>
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]
public class EntityAnimationController : MonoBehaviour
{
    protected Animator animator;
    protected Entity entity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();

        entity.OnMoveStateChanged += MovingAnimation;
        entity.OnDeath += DeathAnimation;
        entity.OnAttacked += AttackAnimation;
        GameManager.Instance.OnGameStarted += GameRestarted;
    }

    private void OnDestroy()
    {
        entity.OnMoveStateChanged -= MovingAnimation;
        entity.OnDeath -= DeathAnimation;
        entity.OnAttacked -= AttackAnimation;
        GameManager.Instance.OnGameStarted -= GameRestarted;
    }

    /// <summary> 걷는 애니메이션을 제생합니다. </summary>
    private void MovingAnimation(object sender, MoveStateEventArgs args)
    {
        if (args.IsMoving)
            animator.SetTrigger("MoveTrg");
        else
            animator.SetTrigger("StopTrg");
    }

    /// <summary> 죽음 애니메이션을 재생합니다. </summary>
    private void DeathAnimation(object sender, EventArgs args)
    {
        animator.SetTrigger("DeathTrg");
        animator.SetBool("Death", true);
    }

    /// <summary> 공격했을 때 애니메이션을 재생합니다. </summary>
    private void AttackAnimation(object sender, AttackEventArgs args)
    {
        animator.SetTrigger("AttackTrg");
    }

    /// <summary> 게임이 재시작되었을 때, Idle 상태로 변경합니다. </summary>
    private void GameRestarted(object sender, EventArgs args)
    {
        animator.SetBool("Death", false);
        animator.SetTrigger("StopTrg");
    }
}
