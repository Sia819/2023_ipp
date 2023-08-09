using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private readonly WaitForSeconds firingDuration = new WaitForSeconds(0.05f);
    private readonly WaitForSeconds fireRateDuration = new WaitForSeconds(0.2f);

    void Awake()
    {
        player = GetComponent<Player>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        if (player.IsAlive == false) return;

        /////////////// Laser trail calculate ///////////////
        if (lineRenderer.enabled)
        {
            // �ѱ��� ���۰� ���� �������� ���� ���� ������ ray�� ����
            Vector3 direction = (gunEnd.position - gunStart.position);
            Ray ray = new Ray(gunEnd.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {// �浹�� ���������� ������ �׸����� ���� ������ ����
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hit.point); // �浹 ��ġ

                if (isFiringCanAttackable && hit.collider.TryGetComponent<Monster>(out Monster monster))
                {
                    monster.CurrentHp -= player.Damage;
                    isFiringCanAttackable = false;      // �ߺ� ���� ����
                    Instantiate(hitParticle.gameObject, hit.point, player.transform.rotation * Quaternion.Euler(0, 180, 0));
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
        if (player.IsAlive == false) return;

        if (Input.GetMouseButton(0))
        {
            if (!isFiring)
            {
                isFiring = true;
                StartCoroutine(FireLaser());
            }
        }
        else
        {
            isFiring = false; // �ڷ�ƾ�� �ݺ��� �ߴ��մϴ�
            lineRenderer.enabled = false; // ������ �����
        }
    }

    IEnumerator FireLaser()
    {
        while (isFiring)
        {
            // �������� �Һ��� ǥ���մϴ�.
            lineRenderer.enabled = true;
            player.GunLight.SetActive(true);
            Instantiate(gunParticle.gameObject, player.GunFlareTransform)?.transform.SetParent(this.transform);
            yield return firingDuration;

            // �������� ����ϴ�.
            lineRenderer.enabled = false;
            player.GunLight.SetActive(false);
            yield return fireRateDuration;

            isFiringCanAttackable = true; // �ߺ����� ����
        }
    }
}
