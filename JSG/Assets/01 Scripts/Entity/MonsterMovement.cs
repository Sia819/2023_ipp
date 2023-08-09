using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(NavMeshAgent))]
public class MonsterMovement : MonoBehaviour
{
    private Monster monster;
    private Player player;              // �÷��̾��� Transform
    private NavMeshAgent navAgent;      // NavMesh Agent ������Ʈ
    private readonly WaitForSeconds pathUpdateRate = new WaitForSeconds(1f);      // ��� ������Ʈ �� (�� ����)

    void Start()
    {
        monster = GetComponent<Monster>();
        navAgent = GetComponent<NavMeshAgent>(); // Nav Mesh Agent ������Ʈ ��������
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // ��ֹ� ȸ�� ǰ�� ����
        player = GameManager.Instance.Player;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (GameManager.Instance.IsPlaying && monster.IsAlive)
        {
            navAgent.SetDestination(player.transform.position); // �÷��̾� ��ġ�� ������ ����
            yield return pathUpdateRate;
        }
        this.navAgent.isStopped = true;
    }
}
