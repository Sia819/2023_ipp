using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; set; } = 5f;

    private Rigidbody rb;
    private Animator anim;

    private float moveHorizontal;          // Player 움직임 수평 값
    private float moveVertical;            // Player 움직임 수직 값
    private float speedCorrectionY = 1.2f; // Player movement 세로축 보정 값
    private Vector3 movement;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        /////////////// 움직임 애니메이션 재생 ///////////////

        if (!(moveHorizontal == 0 && moveVertical == 0) && !isMoving)
        {
            anim.SetTrigger("Move");
            isMoving = true;
        }
        else if (moveHorizontal == 0 && moveVertical == 0 && isMoving)
        {
            anim.SetTrigger("Stop");
            isMoving = false;
        }

        /////////////// Look at mouse ///////////////

        // 카메라에서 마우스 커서로 ray cast 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ray cast 수행 시 100미터 안에서 무언가가 부딪히면
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // ray hit point를 향하도록 Player를 회전합니다.
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }

    private void FixedUpdate()
    {
        /////////////// Player movement ///////////////

        // 키보드 WASD 값 가져오기
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // 움직임 값 연산
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical * speedCorrectionY);
        
        // Rigidbody를 통한 물리 움직임
        rb.MovePosition(transform.position + (movement * Speed * Time.deltaTime));
    }
}
