using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void MovingAnimation(bool moving)
    {
        if (moving)
            animator.SetTrigger("Move");
        else
            animator.SetTrigger("Stop");
    }

    private void DeathAnimation()
    {
        animator.SetTrigger("Death");
    }
}
