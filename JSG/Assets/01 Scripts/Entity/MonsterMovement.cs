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

    private void Awake()
    {
        monster = GetComponent<Monster>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // 장애물 회피 품질 설정
        player = GameManager.Instance.Player;
        StartCoroutine(Move());
    }

    /// <summary> 몬스터의 움직임을 구현합니다. </summary>
    private IEnumerator Move()
    {
        yield return new WaitForEndOfFrame();

        while (GameManager.Instance.IsPlaying && monster.IsAlive)
        {
            if (navAgent.enabled == false) { yield return pathUpdateRate; continue; }

            navAgent.SetDestination(player.transform.position); // 플레이어 위치로 목적지 설정
            yield return pathUpdateRate;
        }
        if (navAgent.enabled)
            this.navAgent.isStopped = true;
    }
}
