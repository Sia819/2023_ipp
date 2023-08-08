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
    private Coroutine attackCoroutine;
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
            attackCoroutine = StartCoroutine(Attack(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (monster.IsAlive && other.CompareTag("Player"))
        {
            continuousAttack = false;
        }
    }

    IEnumerator Attack(Collider other)
    {
        do
        {
            GameManager.Instance.Player.CurrentHp -= monster.damage;
            yield return attackTime;
        } while (continuousAttack);
    }
}
