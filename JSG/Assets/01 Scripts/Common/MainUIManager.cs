using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static GameManager;

public class MainUIManager : Singleton<MainUIManager>
{
    [SerializeField] private Canvas mainUICanvas;
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private TMP_Text playerHpPoint;
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private Animator uiAnimator;
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject startGuide;

    // 점수를 부드럽게 변경시키도록 합니다.
    private bool smoothlyChangeScore = true;

    // 점수를 부드럽게 증가시키기 위한 변수
    private int displayScore;   // 
    private int currentScore;
    private float transitionSpeed = 300f; // 0.5초에 점수을 올리기 위한 속도

    void Start()
    {
        // 플레이어 체력 초기설정
        Player player = GameManager.Instance.Player;
        playerHpBar.maxValue = player.MaxHp;    // 최대 체력 설정
        player.OnHpChanged += HpUpdate;
        player.OnDeath += DeathUI;

        // 점수 업데이트 이벤트 함수 등록
        GameManager.Instance.OnGameScoreChanged += gameScoreChange;
        GameManager.Instance.OnStageStarted += StageStarted;

        // 초기 체력값 설정
        playerHpBar.value = player.CurrentHp;
        playerHpPoint.text = $"{player.CurrentHp}/{player.MaxHp}";

        // 게임 초기화 버튼을 눌렀을 때 동작
        restartButton.onClick.AddListener(GameRestarted);

        // 버튼 하이라이팅 동작 오작동 개선, mainUI Canvas에 Focus가 되어있지 않은 경우 재시작 버튼의 하이라이팅이 활성화 되지 않음.
        // 따라서 버튼의 위에 마우스가 오버될 때 캔버스가 포커스 되도록 함.
        EventTrigger eventTrigger = restartButton.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { EventSystem.current.SetSelectedGameObject(mainUICanvas.gameObject); });
        eventTrigger.triggers.Add(entry);
    }

    private void Update()
    {
        if (smoothlyChangeScore && Math.Abs(currentScore - displayScore) != 0)
        {
            // deltaTime을 고려하여 점수 증가
            int direction = currentScore > displayScore ? 1 : -1;
            displayScore += (int)(direction * transitionSpeed * Time.deltaTime);

            if (direction == 1 && displayScore > currentScore || direction == -1 && displayScore < currentScore)
                displayScore = currentScore;
        }
        else
            displayScore = currentScore;

        gameScore.text = $"SCORE: {displayScore}";
    }

    private void gameScoreChange(object sender, GameScoreChangedEventArgs args)
    {
        currentScore = args.GameScore;
        smoothlyChangeScore = args.SmoothlyChange;

        // 점수의 크기에 따라 부드럽게 상승하는 시간 계산
        int scoreDifference = Math.Abs(currentScore - displayScore);
        if (scoreDifference > 200) transitionSpeed = Math.Abs(currentScore - displayScore);
        else transitionSpeed = 300;

    }

    private void HpUpdate(object sender, HpChangedEventArgs args)
    {
        playerHpBar.value = args.CurrentHp;
        playerHpPoint.text = $"{args.CurrentHp}/{args.MaxHp}";
        uiAnimator.SetTrigger("Damaged");
    }

    private void DeathUI(object sender, EventArgs args)
    {
        uiAnimator.SetTrigger("Death");
    }

    private void GameRestarted()
    {
        uiAnimator.SetTrigger("Restart");
        GameManager.Instance.GameRestart();
        startGuide.gameObject.SetActive(true);
    }

    private void StageStarted(object sender, EventArgs args)
    {
        GameManager.Instance.GameScoreSmoothlyChange = true;
        startGuide.gameObject.SetActive(false);
    }
}
