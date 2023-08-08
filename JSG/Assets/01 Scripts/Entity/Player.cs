using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [field: SerializeField] public Transform GunFlareTransform { get; private set; }
    [field: SerializeField] public GameObject GunLight { get; private set; }
}
