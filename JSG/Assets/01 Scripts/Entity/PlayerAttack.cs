using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(Player))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform gunStart;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private ParticleSystem gunParticle;
    [SerializeField] private ParticleSystem hitParticle;

    private Player player;
    private LineRenderer lineRenderer;
    private bool isFiring;
    private bool isFiringCanAttackable;     // 레이저 발생 시점 한번 만 공격하여, 업데이트 문에서 지속적 데미지를 입히는 문제를 방지
    private Coroutine fireLaserCoroutine;
    private readonly WaitForSeconds firingDuration = new WaitForSeconds(0.05f);
    private readonly WaitForSeconds fireRateDuration = new WaitForSeconds(0.1f);

    void Awake()
    {
        player = GetComponent<Player>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        /////////////// Laser trail calculate ///////////////
        if (lineRenderer.enabled)
        {
            // 총구의 시작과 끝을 기준으로 방향 값만 가져와 ray를 생성
            Vector3 direction = (gunEnd.position - gunStart.position).normalized;
            Ray ray = new Ray(gunEnd.position, direction);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 100f))
            {// 충돌한 영역까지만 광선을 그리도록 라인 렌더러 설정
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hit.point); // 충돌 위치
                
                if (isFiringCanAttackable)
                {
                    Instantiate(gunParticle.gameObject, player.gunFlareTransform)?.transform.SetParent(this.transform);

                    if (hit.collider.TryGetComponent<Monster>(out Monster monster))
                    {
                        monster.CurrentHp -= player.damage;
                        isFiringCanAttackable = false;      // 중복 공격 방지
                        Instantiate(hitParticle.gameObject, hit.point, player.transform.rotation * Quaternion.Euler(0, 180, 0));
                    }
                }
            }
            else
            {// 충돌하지 않았으므로 최대 거리로 설정
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
            // 레이저 보이고, 숨깁니다.
            lineRenderer.enabled = true;
            yield return firingDuration;
            lineRenderer.enabled = false;
            yield return fireRateDuration;

            isFiringCanAttackable = true; // 중복공격 방지
        }
    }
}
