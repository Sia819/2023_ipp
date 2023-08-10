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
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                // T�� �ʱ�ȭ ���� �ʾҰų�, ���α׷��� ���� ������ ���� �� �̱��� ������Ʈ�� �����մϴ�.
                if (_instance == null && Time.timeScale != 0) 
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString() + " (Singleton)";
                }
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
            Debug.LogWarning($"{typeof(T).Name}�� �ϳ� �̻� �����մϴ�!");
        }
    }

    
}