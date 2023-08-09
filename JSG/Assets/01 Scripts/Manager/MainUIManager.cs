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
        // 플레이어 체력 초기설정
        Player player = GameManager.Instance.Player;
        playerHpBar.maxValue = player.MaxHp;    // 최대 체력 설정
        player.OnHpChanged += HpUpdate;
        player.OnDeath += DeathUI;

        // 점수 업데이트 이벤트 함수 등록
        GameManager.Instance.OnGameScoreChanged += gameScoreChange;

        // 초기 체력값 설정
        playerHpBar.value = player.CurrentHp;
        playerHpPoint.text = $"{player.CurrentHp}/{player.MaxHp}";

        // 게임 초기화 버튼을 눌렀을 때 동작
        restartButton.onClick.AddListener(OnGameResetted);

        // 버튼 하이라이팅 동작 오작동 개선, mainUI Canvas에 Focus가 되어있지 않은 경우 재시작 버튼의 하이라이팅이 활성화 되지 않음.
        // 따라서 버튼의 위에 마우스가 오버될 때 캔버스가 포커스 되도록 함.
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
