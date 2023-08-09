using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Monster : Entity
{
    [SerializeField] private int ScorePoint = 100;

    private Collider boundCollider;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private readonly WaitForSeconds deathWaitTime = new WaitForSeconds(0.7f);

    private void Start()
    {
        boundCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        this.OnDeath += ScorePointAdd;
        this.OnDeath += () => StartCoroutine(deathAction());
    }

    private void OnDestroy()
    {
        GameManager.Instance.MonsterCount = 0;
    }

    void ScorePointAdd()
    {
        GameManager.Instance.GameScore += ScorePoint;
    }

    IEnumerator deathAction()
    {
        yield return deathWaitTime;
        boundCollider.enabled = false;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY; // rigidbody�� Y�� ������ Ǳ�ϴ�.

        yield return deathWaitTime;
        Destroy(this.gameObject);
    }
}
