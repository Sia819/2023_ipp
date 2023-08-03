using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private Transform player; // �÷��̾��� Transform
    private float updateRate = 1f; // ��� ������Ʈ �� (�� ����)
    private NavMeshAgent agent; // NavMesh Agent ������Ʈ
    private float timer; // ��� ������Ʈ Ÿ�̸�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMesh Agent ������Ʈ ��������
        player = GameManager.Instance.player.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > updateRate) // ������ ������Ʈ �󵵿� �����ϸ�
        {
            agent.SetDestination(player.position); // �÷��̾� ��ġ�� ������ ����
            timer = 0f;
        }
    }
}
