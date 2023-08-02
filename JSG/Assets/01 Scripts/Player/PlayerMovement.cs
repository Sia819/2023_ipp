using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; set; } = 5f;

    private Rigidbody rb;
    private Animator anim;

    private float moveHorizontal;          // Player ������ ���� ��
    private float moveVertical;            // Player ������ ���� ��
    private float speedCorrectionY = 1.2f; // Player movement ������ ���� ��
    private Vector3 movement;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        /////////////// ������ �ִϸ��̼� ��� ///////////////

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

        // ī�޶󿡼� ���콺 Ŀ���� ray cast ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ray cast ���� �� 100���� �ȿ��� ���𰡰� �ε�����
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // ray hit point�� ���ϵ��� Player�� ȸ���մϴ�.
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }

    private void FixedUpdate()
    {
        /////////////// Player movement ///////////////

        // Ű���� WASD �� ��������
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        // ������ �� ����
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical * speedCorrectionY);
        
        // Rigidbody�� ���� ���� ������
        rb.MovePosition(transform.position + (movement * Speed * Time.deltaTime));
    }
}
