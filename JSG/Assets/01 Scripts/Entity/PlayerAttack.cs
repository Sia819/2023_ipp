using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Player 객체가 마우스 클릭에 따라 공격이 가능하도록 합니다.
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform gunStart;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private ParticleSystem gunParticle;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private float attackLate = 0.25f;        // 공격속도

    private Player player;
    private LineRenderer lineRenderer;
    private float attackCooldown = 0.0f;    // 시간 저장변수

    #region Inspector Warning
    private void OnValidate()
    {
        Validate.NullCheck(this, nameof(gunStart));
        Validate.NullCheck(this, nameof(gunEnd));
        Validate.NullCheck(this, nameof(gunParticle));
        Validate.NullCheck(this, nameof(hitParticle));
    }
    #endregion

    private void Awake()
    {
        player = GetComponent<Player>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        player.OnAttacked += GiveDamage;
    }

    private void LateUpdate()
    {
        Entity attackTarget = null;
        Vector3? hitPoint = null;

        /////////////// 레이, 라인렌더러 실시간 변경 연산 ///////////////
        if (player.IsAlive == true)
        {
            // 총구의 시작과 끝을 기준으로 방향 값만 가져와 ray를 생성
            Vector3 direction = (gunEnd.position - gunStart.position).normalized;
            Ray ray = new Ray(gunEnd.position, direction);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {// 충돌한 영역까지만 광선을 그리도록 라인 렌더러 설정
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, hit.point); // 충돌 위치

                // 몬스터에 닿은 경우 attackTarget 에 저장시킵니다. (옵저버 패턴에서 사용)
                if (hit.collider.TryGetComponent<Monster>(out Monster monster))
                {
                    attackTarget = monster;
                    hitPoint = hit.point;
                }
            }
            else
            {// 충돌하지 않았으므로 최대 거리로 설정
                lineRenderer.SetPosition(0, ray.origin);
                lineRenderer.SetPosition(1, ray.origin + ray.direction * 100);
            }
        }

        ///////////////// 공격 ////////////////
        //쿨타임마다만 공격가능
        if (attackCooldown < 0.0f)
        {
            // 왼쪽 마우스가 눌려진 상태에서만 공격할 수 있습니다.
            if (Input.GetMouseButton(0) == false) return;

            // 플레이어가 생존 시 공격 후 쿨다운, 생존하지 않으면 쿨다운 적용
            if (player.IsAlive == true)
            {
                player.ExecuteAttack(attackTarget, player.Damage, hitPoint); // 공격

                // 첫 공격이 시작되었으므로 스테이지를 시작합니다.
                if (GameManager.Instance.IsStageStarted == false)
                    GameManager.Instance.StateStart();
            }

            // 쿨다운을 적용시킵니다.
            attackCooldown = attackLate;
        }
        else
            attackCooldown -= Time.deltaTime;
    }

    /// <summary> 타겟에게 실질적으로 데미지를 입힙니다. </summary>
    private void GiveDamage(object sender, AttackEventArgs args)
    {
        Instantiate(gunParticle.gameObject, player.GunFlareTransform).transform.SetParent(gunEnd);

        if (args.TargetEntity != null)
        {
            args.TargetEntity.CurrentHp -= args.Damage;
            Instantiate(hitParticle.gameObject, args.HitPoint.Value, player.transform.rotation * Quaternion.Euler(0, 180, 0));
        }
    }
}