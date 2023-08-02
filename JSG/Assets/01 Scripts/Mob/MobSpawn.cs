using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawn : MonoBehaviour
{
    [field: SerializeField] public Collider SpawnFloor { get; set; }
    [field: SerializeField] public Collider SpawnHole { get; set; }
    [field: SerializeField] public Transform Player { get; set; }
    [field: SerializeField] public GameObject MobZomBear { get; set; }
    [field: SerializeField] public GameObject MobZomBunny { get; set; }
    [field: SerializeField] public GameObject MobHellephant { get; set; }

    Vector3 SpawnArea { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsPlaying)
        {
            //StartCoroutine(Spawn());
        }
    }

    //IEnumerator Spawn()
    //{
    //    while (GameManager.Instance.IsPlaying)
    //    {
    //        
    //
    //         //Vector3 MapCenter.position.x + 10
    //
    //
    //        //Instantiate()
    //    }
    //}

    //Vector3 GenRandomPoint(Vector3 basePosition)
    //{
    //    float 
    //}
}
