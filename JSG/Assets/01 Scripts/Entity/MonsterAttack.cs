using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Monster))]
[RequireComponent(typeof(Collider))]
public class MonsterAttack : MonoBehaviour
{
    [field: SerializeField] public Collider AttackCollider { get; private set; }

    private Monster monster;
    private readonly WaitForSeconds attackTime = new WaitForSeconds(1f);
    private bool continuousAttack;

    void Awake()
    {
        monster = GetComponent<Monster>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (monster.IsAlive && other.CompareTag("Player"))
        {
            continuousAttack = true;
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (monster.IsAlive && other.CompareTag("Player"))
        {
            continuousAttack = false;
        }
    }

    IEnumerator Attack()
    {
        do
        {
            GameManager.Instance.Player.CurrentHp -= monster.Damage;
            yield return attackTime;
        } while (continuousAttack);
    }
}
