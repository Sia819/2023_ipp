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
    private bool isFiringCanAttackable;     // ������ �߻� ���� �ѹ� �� �����Ͽ�, ������Ʈ ������ ������ �������� ������ ������ ����
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
            // �ѱ��� ���۰� ���� �������� ���� ���� ������ ray�� ����
            Vector3 direction = (gunEnd.position - gunStart.position).normalized;
            Ray ray = new Ray(gunEnd.position, direction);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 100f))
            {// �浹�� ���������� ������ �׸����� ���� ������ ����
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hit.point); // �浹 ��ġ
                
                if (isFiringCanAttackable)
                {
                    Instantiate(gunParticle.gameObject, player.gunFlareTransform)?.transform.SetParent(this.transform);

                    if (hit.collider.TryGetComponent<Monster>(out Monster monster))
                    {
                        monster.CurrentHp -= player.damage;
                        isFiringCanAttackable = false;      // �ߺ� ���� ����
                        Instantiate(hitParticle.gameObject, hit.point, player.transform.rotation * Quaternion.Euler(0, 180, 0));
                    }
                }
            }
            else
            {// �浹���� �ʾ����Ƿ� �ִ� �Ÿ��� ����
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
            // ������ ���̰�, ����ϴ�.
            lineRenderer.enabled = true;
            yield return firingDuration;
            lineRenderer.enabled = false;
            yield return fireRateDuration;

            isFiringCanAttackable = true; // �ߺ����� ����
        }
    }
}
