using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션에서 제어할 프로그램 로직 입니다.
/// </summary>
public class GameOverScore : MonoBehaviour
{
    private int score;

    /// <summary> 애니메이션 효과로, 스코어를 0으로 잠시 숨깁니다. </summary>
    public void ScoreHide()
    {
        score = GameManager.Instance.GameScore;
        GameManager.Instance.GameScoreSmoothlyChange = false;
        GameManager.Instance.GameScore = 0;
    }

    /// <summary> 애니메이션 효과로, 스코어를 다시 표시합니다. </summary>
    public void ScoreShow()
    {
        GameManager.Instance.GameScoreSmoothlyChange = true;
        GameManager.Instance.GameScore = score;
    }
}
