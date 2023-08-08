using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Slider playerHpBar;
    [SerializeField] private TMP_Text playerHpPoint;

    void Start()
    {
        Player player = GameManager.Instance.Player;

        playerHpBar.maxValue = player.maxHp;    // 최대 체력 설정
        player.OnHpChanged += HpBarUpdate;
        player.OnDeath += () => DeathUI();
    }

    private void HpBarUpdate(float currentHp, float maxHp)
    {
        playerHpBar.value = currentHp;
        playerHpPoint.text = $"{currentHp}/{maxHp}";
    }

    IEnumerator DeathUI()
    {
        yield return null;
    }
}
