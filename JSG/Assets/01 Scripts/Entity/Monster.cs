using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class Monster : Entity
{
    Collider collider;
    private Rigidbody rb;
    private NavMeshAgent agent;

    private void Start()
    {
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    internal override void Death()
    {
        StartCoroutine(deathAction());
    }

    IEnumerator deathAction()
    {
        yield return new WaitForSeconds(0.7f);
        collider.enabled = false;
        agent.enabled = false;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        yield return new WaitForSeconds(0.7f);
        Destroy(this.gameObject);
    }
}
