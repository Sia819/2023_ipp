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
            // 총구의 시작과 끝을 기준으로 방향 값만 가져와 ray를 생성
            Vector3 direction = (GunEnd.position - GunStart.position).normalized;
            Ray ray = new Ray(GunEnd.position, direction);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                // 광선을 그리도록 라인 렌더러 설정
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hit.point); // 충돌 위치
            }
            else
            {
                // 충돌하지 않았으므로 최대 거리로 설정
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
            lineRenderer.enabled = false; // 레이저 숨기기
        }
    }

    IEnumerator FireLaser()
    {
        while (isFiring)
        {
            // 레이저 보이기
            lineRenderer.enabled = true;

            // 0.2초 기다림
            yield return new WaitForSeconds(0.05f);

            // 레이저 숨기기
            lineRenderer.enabled = false;

            // 0.2초 기다림
            yield return new WaitForSeconds(0.1f);
        }
    }
}
