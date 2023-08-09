using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Monster))]
public class MonsterUIManager : MonoBehaviour
{
    [SerializeField] private Transform monsterUI;
    [SerializeField] private Slider monsterHpBar;
    [SerializeField] private Vector3 HPBAR_POS_OFFSET = new(0f, -5f, 0f);

    private Monster monster;
    private Camera mainCamera;
    private Coroutine hideHpBarCoroutine;


    void Start()
    {
        monster = GetComponent<Monster>();
        mainCamera = Camera.main;

        monster.OnHpChanged += HpBarUpdate;

        monsterHpBar.maxValue = monster.MaxHp;
        monsterHpBar.value = monster.CurrentHp;

        // 초기 상태에서 체력바와 캔버스를 숨깁니다.
        monsterUI.gameObject.SetActive(false);
    }

    // 매 업데이트마다 몬스터의 체력바의 위치를 카메라를 향하도록 보정합니다.
    void LateUpdate()
    {
        // 위치를 기반으로 체력바의 초기 위치를 설정합니다.
        Vector3 initialPosition = this.transform.localPosition + HPBAR_POS_OFFSET;

        // 몬스터 위치에서 카메라 방향으로 조금 이동한 위치로 체력바를 이동합니다.
        Vector3 cameraDirection = (initialPosition - mainCamera.transform.localPosition).normalized;
        monsterUI.transform.position = initialPosition + cameraDirection * 1f;

        // 체력바가 항상 카메라를 바라보게 합니다.
        monsterUI.transform.LookAt(initialPosition + mainCamera.transform.rotation * Vector3.forward,
                                         mainCamera.transform.rotation * Vector3.up);

        // 체력바의 회전을 Y축을 중심으로 180도 회전시킵니다.
        monsterUI.transform.Rotate(0, 180, 0);
    }


    void HpBarUpdate(float currentHp, float maxHp)
    {
        // 체력바를 활성화합니다.
        monsterUI.gameObject.SetActive(true);
        monsterHpBar.value = currentHp;

        // 체력이 0이라면 체력바를 즉시 숨깁니다.
        if (currentHp <= 0)
        {
            monsterUI.gameObject.SetActive(false);
            return;
        }

        // 이미 실행 중인 숨기기 코루틴이 있다면 중지시킵니다.
        if (hideHpBarCoroutine != null)
            StopCoroutine(hideHpBarCoroutine);

        // 새로운 숨기기 코루틴을 시작합니다.
        hideHpBarCoroutine = StartCoroutine(HideHpBarAfterDelay());
    }

    IEnumerator HideHpBarAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        monsterUI.gameObject.SetActive(false);
    }
}
