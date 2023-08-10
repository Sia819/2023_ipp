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

        // �ʱ� ���¿��� ü�¹ٿ� ĵ������ ����ϴ�.
        monsterUI.gameObject.SetActive(false);
    }

    // �� ������Ʈ���� ������ ü�¹��� ��ġ�� ī�޶� ���ϵ��� �����մϴ�.
    void LateUpdate()
    {
        // ��ġ�� ������� ü�¹��� �ʱ� ��ġ�� �����մϴ�.
        Vector3 initialPosition = this.transform.localPosition + HPBAR_POS_OFFSET;

        // ���� ��ġ���� ī�޶� �������� ���� �̵��� ��ġ�� ü�¹ٸ� �̵��մϴ�.
        Vector3 cameraDirection = (initialPosition - mainCamera.transform.localPosition).normalized;
        monsterUI.transform.position = initialPosition + cameraDirection * 1f;

        // ü�¹ٰ� �׻� ī�޶� �ٶ󺸰� �մϴ�.
        monsterUI.transform.LookAt(initialPosition + mainCamera.transform.rotation * Vector3.forward,
                                         mainCamera.transform.rotation * Vector3.up);

        // ü�¹��� ȸ���� Y���� �߽����� 180�� ȸ����ŵ�ϴ�.
        monsterUI.transform.Rotate(0, 180, 0);
    }


    void HpBarUpdate(float currentHp, float maxHp)
    {
        // ü�¹ٸ� Ȱ��ȭ�մϴ�.
        monsterUI.gameObject.SetActive(true);
        monsterHpBar.value = currentHp;

        // ü���� 0�̶�� ü�¹ٸ� ��� ����ϴ�.
        if (currentHp <= 0)
        {
            monsterUI.gameObject.SetActive(false);
            return;
        }

        // �̹� ���� ���� ����� �ڷ�ƾ�� �ִٸ� ������ŵ�ϴ�.
        if (hideHpBarCoroutine != null)
            StopCoroutine(hideHpBarCoroutine);

        // ���ο� ����� �ڷ�ƾ�� �����մϴ�.
        hideHpBarCoroutine = StartCoroutine(HideHpBarAfterDelay());
    }

    IEnumerator HideHpBarAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        monsterUI.gameObject.SetActive(false);
    }
}
