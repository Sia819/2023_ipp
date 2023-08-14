using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

#region CustomEventArgs Class
public class GameScoreChangedEventArgs : EventArgs
{
    public int GameScore { get; }
    public bool SmoothlyChange { get; }

    public GameScoreChangedEventArgs(int gameScore, bool smoothlyChange = true)
    {
        GameScore = gameScore;
        SmoothlyChange = smoothlyChange;
    }
}
#endregion


public class GameManager : Singleton<GameManager>
{
    public delegate void GameScoreChangedHandler(object sender, GameScoreChangedEventArgs args);
    public event GameScoreChangedHandler OnGameScoreChanged;

    /// <summary>
    /// 게임을 처음 실행시켰거나, 재시작했을 때 호출됩니다.
    /// </summary>
    public delegate void GameStarted(object sender, EventArgs args);
    public event GameStarted OnGameStarted;

    /// <summary>
    /// 게임이 시작되고, 플레이어가 WASD 또는 공격을 했을 때 호출됩니다.
    /// 이 시점부터 몬스터가 스폰됩니다.
    /// </summary>
    public delegate void StageStarted(object sender, EventArgs args);
    public event StageStarted OnStageStarted;

    [field: SerializeField] public Player Player { get; private set; }

    public bool IsPlaying { get; set; }

    public int MonsterCount { get; set; }

    public bool GameScoreSmoothlyChange { get; set; } = true;

    public bool IsStageStarted { get; private set; }

    public int GameScore
    {
        get => this.gameScore;
        set
        {
            this.gameScore = value;
            OnGameScoreChanged?.Invoke(this, new GameScoreChangedEventArgs(this.gameScore, GameScoreSmoothlyChange));
        }
    }

    private int gameScore;

    void Start()
    {
        IsPlaying = true;
        OnGameStarted.Invoke(this, EventArgs.Empty);
    }

    public void GameRestart()
    {
        IsPlaying = true;
        GameScoreSmoothlyChange = false;
        IsStageStarted = false;
        GameScore = 0;
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public void StateStart()
    {
        OnStageStarted?.Invoke(this, EventArgs.Empty);
        IsStageStarted = true;
    }
}
