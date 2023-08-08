using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private TMP_Text playerHpPoint;
    [SerializeField] private TMP_Text gameScore;
    [SerializeField] private Animator uiAnimator;

    void Start()
    {
        Player player = GameManager.Instance.Player;
        playerHpBar.maxValue = player.maxHp;    // 최대 체력 설정
        player.OnHpChanged += HpUpdate;
        player.OnDeath += DeathUI;

        GameManager.Instance.OnGameScoreChanged += gameScoreChange;
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
}
