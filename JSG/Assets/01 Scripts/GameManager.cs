using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameObject player;

    [HideInInspector] public bool isPlaying = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isPlaying = true;
    }
}
