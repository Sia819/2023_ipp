using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region CustomEventArgs Class
public class GameScoreChangedEventArgs : EventArgs
{
    public int GameScore { get; }

    public GameScoreChangedEventArgs(int gameScore)
    {
        GameScore = gameScore;
    }
}
#endregion


public class GameManager : Singleton<GameManager>
{
    public delegate void GameScoreChangedHandler(int gameScore);
    public event GameScoreChangedHandler OnGameScoreChanged;

    public delegate void GameRestarted(object sender, EventArgs args);
    public event GameRestarted OnGameRestarted;

    [field: SerializeField] public Player Player { get; private set; }

    public bool IsPlaying { get; set; }

    public int MonsterCount { get; set; }

    public int GameScore
    {
        get => this.gameScore;
        set
        {
            this.gameScore = value;
            OnGameScoreChanged?.Invoke(this.gameScore);
        }
    }

    private int gameScore;

    void Start()
    {
        IsPlaying = true;
    }

    public void GameRestart()
    {
        IsPlaying = true;
        GameScore = 0;
        OnGameRestarted?.Invoke(this, EventArgs.Empty);
    }
}
