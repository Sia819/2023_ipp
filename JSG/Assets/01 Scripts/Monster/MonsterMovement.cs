using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private Transform player; // 플레이어의 Transform
    private float updateRate = 1f; // 경로 업데이트 빈도 (초 단위)
    private NavMeshAgent agent; // NavMesh Agent 컴포넌트
    private float timer; // 경로 업데이트 타이머

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // NavMesh Agent 컴포넌트 가져오기
        player = GameManager.Instance.player.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > updateRate) // 지정된 업데이트 빈도에 도달하면
        {
            agent.SetDestination(player.position); // 플레이어 위치로 목적지 설정
            timer = 0f;
        }
    }
}
