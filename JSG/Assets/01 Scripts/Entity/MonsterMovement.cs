using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(NavMeshAgent))]
public class MonsterMovement : MonoBehaviour
{
    private Monster monster;
    private Player player;              // 플레이어의 Transform
    private NavMeshAgent navAgent;      // NavMesh Agent 컴포넌트
    private float updateRate = 1f;      // 경로 업데이트 빈도 (초 단위)
    private float timer;                // 경로 업데이트 타이머

    void Start()
    {
        monster = GetComponent<Monster>();
        navAgent = GetComponent<NavMeshAgent>(); // Nav Mesh Agent 컴포넌트 가져오기
        // TODO : 나중에 우선순위 다시 확인하기
        //navAgent.avoidancePriority = 10; // 회피 우선순위 설정
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // 장애물 회피 품질 설정
        player = GameManager.Instance.Player;
    }

    void Update()
    {
        if (monster.IsAlive)
        {
            timer += Time.deltaTime;
            if (timer > updateRate) // 지정된 업데이트 빈도에 도달하면
            {
                navAgent.SetDestination(player.transform.position); // 플레이어 위치로 목적지 설정
                timer = 0f;
            }
        }
    }
}
