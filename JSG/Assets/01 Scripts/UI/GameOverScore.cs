using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이션에서 제어할 프로그램 로직 입니다.
/// </summary>
public class GameOverScore : MonoBehaviour
{
    private int score;

    public void ScoreHide()
    {
        score = GameManager.Instance.GameScore;
        GameManager.Instance.GameScoreSmoothlyChange = false;
        GameManager.Instance.GameScore = 0;
    }

    public void ScoreShow()
    {
        GameManager.Instance.GameScoreSmoothlyChange = true;
        GameManager.Instance.GameScore = score;
    }

}
