using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [field: SerializeField] public Player Player { get; private set; }

    [HideInInspector] public bool isPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("GameManager�� �ϳ� �̻� �����մϴ�!");
        }
    }

    void Start()
    {
        isPlaying = true;
    }
}
