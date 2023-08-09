using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public delegate void GameScoreChanged(int score);
    public event GameScoreChanged OnGameScoreChanged;

    public delegate void GameResetted();
    public event GameResetted OnGameResetted;

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

    public void GameReset()
    {
        IsPlaying = true;
        GameScore = 0;
        OnGameResetted?.Invoke();
    }
}
