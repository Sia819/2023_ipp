using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(Collider))]
public class MonsterAttack : MonoBehaviour
{
    [field: SerializeField] public Collider AttackCollider { get; private set; }

    private Monster monster;

    void Start()
    {
        monster = GetComponent<Monster>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Player.CurrentHp -= monster.damage;
        }
    }
}
