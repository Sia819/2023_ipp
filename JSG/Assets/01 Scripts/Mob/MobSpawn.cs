using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobRespawn : MonoBehaviour
{
    [field: SerializeField] public Transform Player { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsPlaying)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
