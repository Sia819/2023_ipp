using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public delegate void GameScoreChanged(int score);
    public event GameScoreChanged OnGameScoreChanged;

    [field: SerializeField] public Player Player { get; private set; }

    [HideInInspector] public bool isPlaying = false;


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
        isPlaying = true;
    }
}
