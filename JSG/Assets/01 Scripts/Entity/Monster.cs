using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Monster : Entity
{
    [SerializeField] private int ScorePoint = 100;
    [SerializeField] private Collider boundCollider;
    [SerializeField] private Collider attackCollider;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private readonly WaitForSeconds deathWaitTime = new WaitForSeconds(0.7f);

    #region Inspector Warning
    private void OnValidate()
    {
        Validate.NullCheck(this, nameof(boundCollider));
        Validate.NullCheck(this, nameof(attackCollider));
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        this.OnDeath += DeathAction;
    }

    private void OnDestroy()
    {
        GameManager.Instance.MonsterCount--;
        this.OnDeath -= DeathAction;
    }

    private void DeathAction(object sender, EventArgs args)
    {
        GameManager.Instance.GameScore += ScorePoint;   // ScorePointAdd
        StartCoroutine(DeathProcess());
    }

    /// <summary> Monster가 죽었을 때 처리해야 할 작업 </summary>
    private IEnumerator DeathProcess()
    {
        yield return deathWaitTime;
        attackCollider.enabled = false;
        boundCollider.enabled = false;
        agent.enabled = false;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY; // rigidbody의 Y축 고정을 풉니다.

        yield return deathWaitTime;
        Destroy(this.gameObject);
    }
}
