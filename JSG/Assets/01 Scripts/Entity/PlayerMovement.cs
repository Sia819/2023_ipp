using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Rigidbody rb;
    private Player player;
    private float speedCorrectionY = 1.2f; // Player movement 세로축 보정 값

    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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

    private void FixedUpdate()
    {
        if (player.IsAlive == false) return;

        /////////////// Player movement ///////////////

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 움직임 속성 값 업데이트
        if (!(moveHorizontal == 0 && moveVertical == 0))
            player.IsMoving = true;
        else if (moveHorizontal == 0 && moveVertical == 0)
            player.IsMoving = false;

        // 물리 움직임 구현
        Vector3 destination = new Vector3(moveHorizontal, 0.0f, moveVertical * speedCorrectionY); // Y축 보정값 
        rb.MovePosition(transform.position + (destination * speed * Time.deltaTime));
    }
}
