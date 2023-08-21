using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 움직임을 제어할 수 있습니다.
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody rb;
    private Player player;
    private readonly float speedCorrectionY = 1.2f; // Player movement 세로축 보정 값

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary> 매 프레임마다 캐릭터가 마우스의 방향을 바라보도록 합니다. </summary>
    private void Update()
    {
        if (player.IsAlive == false) return;

        /////////////// Look at mouse ///////////////

        // 카메라에서 마우스 커서로 ray cast 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        //Physics.Raycast(ray, out RaycastHit hit, 100f)
        // ray cast 수행 시 100미터 안에서 무언가가 부딪히면
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Floor"))
            {
                // ray floor hit point를 향하도록 Player를 회전
                transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            }
        }
    }

    /// <summary> 방향키에 따른 방향으로 캐릭터를 물리적으로 움직이도록 합니다. </summary>
    private void FixedUpdate()
    {
        if (player.IsAlive == false) return;

        /////////////// Player movement ///////////////

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 움직임 속성 값 업데이트
        if (moveHorizontal == 0 && moveVertical == 0)
            player.IsMoving = false;
        else if (!(moveHorizontal == 0 && moveVertical == 0))
        {
            player.IsMoving = true;
            // 첫 움직임이 시작되었으므로 스테이지를 시작합니다.
            if (GameManager.Instance.IsStageStarted == false)
                GameManager.Instance.StateStart();
        }

        // 물리 움직임 구현
        Vector3 destination = new Vector3(moveHorizontal, 0.0f, moveVertical * speedCorrectionY); // Y축 보정값 
        rb.MovePosition(transform.position + (speed * Time.fixedDeltaTime * destination));
    }
}
