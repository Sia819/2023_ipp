using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool IsPlaying = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        IsPlaying = true;
    }
}
