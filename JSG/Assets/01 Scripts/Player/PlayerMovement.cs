using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    
    private Rigidbody rb;
    private Animator anim;
    private Vector3 movement;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 키보드 WASD 값 가져오기
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // 움직임 애니메이션 재생
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

        LookAtMouse();
    }

    private void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector3 direction)
    {
        rb.MovePosition(transform.position + (direction * speed * Time.deltaTime));
    }

    void LookAtMouse()
    {
        // 카메라에서 마우스 커서로 레이 캐스트 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 레이 캐스트를 수행하고 100미터 안에서 무언가가 부딪히면
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // Rotate the object to face the hit point
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }
}
