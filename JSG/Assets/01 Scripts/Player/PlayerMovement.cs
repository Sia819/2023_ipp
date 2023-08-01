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
        // Ű���� WASD �� ��������
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // ������ �ִϸ��̼� ���
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
        // ī�޶󿡼� ���콺 Ŀ���� ���� ĳ��Ʈ ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ���� ĳ��Ʈ�� �����ϰ� 100���� �ȿ��� ���𰡰� �ε�����
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // Rotate the object to face the hit point
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }
}
