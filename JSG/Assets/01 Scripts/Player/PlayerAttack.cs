using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerAttack : MonoBehaviour
{
    [field: SerializeField] public Transform GunStart { get; set; }
    [field: SerializeField] public Transform GunEnd { get; set; }
    
    private LineRenderer lineRenderer;
    private bool isFiring;
    private Coroutine fireLaserCoroutine;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        /////////////// Laser trail calculate ///////////////
        if (lineRenderer.enabled)
        {
            // �ѱ��� ���۰� ���� �������� ���� ���� ������ ray�� ����
            Vector3 direction = (GunEnd.position - GunStart.position).normalized;
            Ray ray = new Ray(GunEnd.position, direction);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // ������ �׸����� ���� ������ ����
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hit.point); // �浹 ��ġ
            }
            else
            {
                // �浹���� �ʾ����Ƿ� �ִ� �Ÿ��� ����
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isFiring)
            {
                isFiring = true;
                fireLaserCoroutine = StartCoroutine(FireLaser());
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFiring = false;
            if (fireLaserCoroutine != null)
            {
                StopCoroutine(fireLaserCoroutine);
            }
            lineRenderer.enabled = false; // ������ �����
        }
    }

    IEnumerator FireLaser()
    {
        while (isFiring)
        {
            // ������ ���̱�
            lineRenderer.enabled = true;

            // 0.2�� ��ٸ�
            yield return new WaitForSeconds(0.05f);

            // ������ �����
            lineRenderer.enabled = false;

            // 0.2�� ��ٸ�
            yield return new WaitForSeconds(0.1f);
        }
    }
}
