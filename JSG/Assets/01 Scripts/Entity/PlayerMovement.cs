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
    private float speedCorrectionY = 1.2f; // Player movement ������ ���� ��

    void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        /////////////// Look at mouse ///////////////

        // ī�޶󿡼� ���콺 Ŀ���� ray cast ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ray cast ���� �� 100���� �ȿ��� ���𰡰� �ε�����
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            // ray floor hit point�� ���ϵ��� Player�� ȸ��
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }

    private void FixedUpdate()
    {
        /////////////// Player movement ///////////////

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // ������ �Ӽ� �� ������Ʈ
        if (!(moveHorizontal == 0 && moveVertical == 0))
            player.IsMoving = true;
        else if (moveHorizontal == 0 && moveVertical == 0)
            player.IsMoving = false;

        // ���� ������ ����
        Vector3 destination = new Vector3(moveHorizontal, 0.0f, moveVertical * speedCorrectionY); // Y�� ������ 
        rb.MovePosition(transform.position + (destination * speed * Time.deltaTime));
    }
}
