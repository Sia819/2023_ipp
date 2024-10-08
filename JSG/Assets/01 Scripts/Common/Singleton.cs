using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            _instance ??= FindObjectOfType<T>();
            // T가 초기화 되지 않았거나, 프로그램이 종료 중이지 않을 때 경고를 표시합니다.
            if (_instance == null && Time.timeScale != 0)
            {
                Debug.LogWarning($"{typeof(T)}가 존재하지 않습니다!");
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this);
            Debug.LogWarning($"{typeof(T).Name}가 하나 이상 존재합니다!", this);
        }
    }
}