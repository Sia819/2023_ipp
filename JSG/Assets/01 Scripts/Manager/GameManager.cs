using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public Player Player { get; private set; }

    [HideInInspector] public bool isPlaying = false;

    public int score = 0;

    void Start()
    {
        isPlaying = true;
    }
}
