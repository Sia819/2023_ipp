using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Entity;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]
public class EntityAnimationController : MonoBehaviour
{
    private Animator animator;
    private Entity entity;

    void Start()
    {
        animator = GetComponent<Animator>();
        entity = GetComponent<Entity>();
        entity.OnMoveStateChanged += MovingAnimation;
        entity.OnDeath += DeathAnimation;
        GameManager.Instance.OnGameResetted += OnResetted; 
    }

    private void OnDestroy()
    {
        entity.OnMoveStateChanged -= MovingAnimation;
        entity.OnDeath -= DeathAnimation;
        GameManager.Instance.OnGameResetted -= OnResetted;
    }

    private void MovingAnimation(object sender, MoveStateEventArgs args)
    {
        if (args.IsMoving)
            animator.SetTrigger("MoveTrg");
        else
            animator.SetTrigger("StopTrg");
    }

    private void DeathAnimation(object sender, EventArgs args)
    {
        animator.SetTrigger("DeathTrg");
        animator.SetBool("Death", true);
    }

    private void OnResetted()
    {
        animator?.SetBool("Death", false);
        animator?.SetTrigger("StopTrg");
    }
}
