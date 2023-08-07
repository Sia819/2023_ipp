using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public Transform gunFlareTransform;
    public GameObject gunLight;

    internal override void Death()
    {
        throw new System.NotImplementedException();
    }
}
