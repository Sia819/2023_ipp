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
    private readonly WaitForSeconds pathUpdateRate = new WaitForSeconds(1f);      // 경로 업데이트 빈도 (초 단위)

    void Start()
    {
        monster = GetComponent<Monster>();
        navAgent = GetComponent<NavMeshAgent>(); // Nav Mesh Agent 컴포넌트 가져오기
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // 장애물 회피 품질 설정
        player = GameManager.Instance.Player;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (GameManager.Instance.IsPlaying && monster.IsAlive)
        {
            navAgent.SetDestination(player.transform.position); // 플레이어 위치로 목적지 설정
            yield return pathUpdateRate;
        }
        this.navAgent.isStopped = true;
    }
}
