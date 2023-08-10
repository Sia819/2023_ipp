using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainUIManager : Singleton<MainUIManager>
{
    [SerializeField] private Canvas mainUICanvas;
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private TMP_Text playerHpPoint;
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private Button restartButton;

    void Start()
    {
        // �÷��̾� ü�� �ʱ⼳��
        Player player = GameManager.Instance.Player;
        playerHpBar.maxValue = player.MaxHp;    // �ִ� ü�� ����
        player.OnHpChanged += HpUpdate;
        player.OnDeath += DeathUI;

        // ���� ������Ʈ �̺�Ʈ �Լ� ���
        GameManager.Instance.OnGameScoreChanged += gameScoreChange;

        // �ʱ� ü�°� ����
        playerHpBar.value = player.CurrentHp;
        playerHpPoint.text = $"{player.CurrentHp}/{player.MaxHp}";

        // ���� �ʱ�ȭ ��ư�� ������ �� ����
        restartButton.onClick.AddListener(OnGameResetted);

        // ��ư ���̶����� ���� ���۵� ����, mainUI Canvas�� Focus�� �Ǿ����� ���� ��� ����� ��ư�� ���̶������� Ȱ��ȭ ���� ����.
        // ���� ��ư�� ���� ���콺�� ������ �� ĵ������ ��Ŀ�� �ǵ��� ��.
        EventTrigger eventTrigger = restartButton.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { EventSystem.current.SetSelectedGameObject(mainUICanvas.gameObject); });
        eventTrigger.triggers.Add(entry);
    }

    private void gameScoreChange(int score)
    {
        gameScore.text = $"SCORE: {score}";
    }

    private void HpUpdate(float currentHp, float maxHp)
    {
        playerHpBar.value = currentHp;
        playerHpPoint.text = $"{currentHp}/{maxHp}";
        uiAnimator.SetTrigger("Damaged");
    }

    private void DeathUI()
    {
        uiAnimator.SetTrigger("Death");
    }

    private void OnGameResetted()
    {
        uiAnimator.SetTrigger("Restart");
        GameManager.Instance.GameReset();
    }
}
