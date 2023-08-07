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
    private float updateRate = 1f;      // ��� ������Ʈ �� (�� ����)
    private float timer;                // ��� ������Ʈ Ÿ�̸�

    void Start()
    {
        monster = GetComponent<Monster>();
        navAgent = GetComponent<NavMeshAgent>(); // Nav Mesh Agent ������Ʈ ��������
        // TODO : ���߿� �켱���� �ٽ� Ȯ���ϱ�
        //navAgent.avoidancePriority = 10; // ȸ�� �켱���� ����
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // ��ֹ� ȸ�� ǰ�� ����
        player = GameManager.Instance.Player;
    }

    void Update()
    {
        if (monster.IsAlive)
        {
            timer += Time.deltaTime;
            if (timer > updateRate) // ������ ������Ʈ �󵵿� �����ϸ�
            {
                navAgent.SetDestination(player.transform.position); // �÷��̾� ��ġ�� ������ ����
                timer = 0f;
            }
        }
    }
}
