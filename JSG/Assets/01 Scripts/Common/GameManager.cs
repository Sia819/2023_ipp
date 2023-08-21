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
    /// <summary>
    /// 게임점수가 변경되었을 때 호출됩니다.
    /// </summary>
    public delegate void GameScoreChangedHandler(object sender, GameScoreChangedEventArgs args);
    public event GameScoreChangedHandler OnGameScoreChanged;

    /// <summary>
    /// 게임을 처음 실행시켰거나, 재시작했을 때 호출됩니다.
    /// </summary>
    public delegate void GameStarted(object sender, EventArgs args);
    public event GameStarted OnGameStarted;

    /// <summary>
    /// 게임의 현재 스테이지가 시작되었을 때 호출됩니다. 
    /// 게임이 재시작 되더라도 바로 진행되지 않는 요소를 스테이지로 관리할 수 있습니다.
    /// 예를들어 게임이 시작되었을 때 몬스터가 나중에 나오게 하는 활용 방법이 존재합니다.
    /// </summary>
    public delegate void StageStarted(object sender, EventArgs args);
    public event StageStarted OnStageStarted;

    /// <summary>
    /// 게임의 메인 플레이어 입니다.
    /// </summary>
    [field: SerializeField] public Player Player { get; private set; }

    /// <summary>
    /// 게임이 진행 중인지 여부입니다.
    /// </summary>
    public bool IsPlaying
    {
        get => isPlaying;
        private set
        {
            this.isPlaying = value;
            if (isPlaying == true)
                OnGameStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 현재 스폰된 몬스터의 수
    /// </summary>
    public int MonsterCount { get; set; }

    /// <summary>
    /// 게임 스코어가 부드럽게 변하는지의 여부입니다.
    /// <see cref="GameScore"/>를 변경할 때 결정짓습니다.
    /// </summary>
    public bool GameScoreSmoothlyChange { get; set; } = true;

    /// <summary>
    /// 스테이지가 시작되었느닞의 여부입니다.
    /// </summary>
    public bool IsStageStarted
    {
        get => isStageStarted;
        private set
        {
            this.isStageStarted = value;
            if (isStageStarted == true)
                OnStageStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 게임에서 얻은 총 점수 값 입니다. <para/>
    /// 점수가 변경되면 <see cref="OnGameScoreChanged"/> 이벤트가 호출됩니다.
    /// </summary>
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
    private bool isPlaying;
    private bool isStageStarted;

    #region Inspector Warning
    private void OnValidate()
    {
        Validate.NullCheck(this, nameof(Player));
    }
    #endregion

    private void Start()
    {
        IsPlaying = true;
    }

    /// <summary>
    /// 호출 시 게임을 초기화 합니다.
    /// </summary>
    public void GameRestart()
    {
        IsPlaying = true;
        GameScoreSmoothlyChange = false;
        IsStageStarted = false;
        GameScore = 0;
    }

    /// <summary>
    /// 호출 시 스테이지가 시작되며, 게임이 진행됩니다.
    /// </summary>
    public void StateStart()
    {
        IsStageStarted = true;
    }

    /// <summary>
    /// 호출 시 게임이 종료됩니다.
    /// </summary>
    public void GameSet()
    {
        IsPlaying = false;
    }
}
