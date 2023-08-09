using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public delegate void MoveStateChangedHandler(bool moving);
    public delegate void HpChangedHandler(float currentHp, float maxHp);
    public delegate void DeathHandler();

    /// <summary> Entity�� �̵� ���� ���� �� �̺�Ʈ </summary>
    public event MoveStateChangedHandler OnMoveStateChanged;
    /// <summary> Entity�� ü�� ���� �� �̺�Ʈ </summary>
    public event HpChangedHandler OnHpChanged;
    public event DeathHandler OnDeath;

    [field: SerializeField] public float MaxHp { get; set; } = 100f;
    [field: SerializeField] public float Damage { get; set; } = 20f;

    private float currentHp;
    private bool isMoving;

    /// <summary> Entity�� ���� ����ִ����� �����Դϴ�. </summary>
    public bool IsAlive => currentHp > 0f;

    /// <summary> ���� Entity�� ü���Դϴ�. </summary>
    public float CurrentHp
    {
        get => this.currentHp;
        set
        {
            // ���� ���� �����Ϸ��� �ϰų�, ü���� 0���� ���� ������ �� �����ϴ�.
            if (this.currentHp == value) return;            
            if (value < 0 && this.currentHp == 0) return;   
            
            if (value > 0)
                this.currentHp = value;
            else
            {
                this.currentHp = 0f;
                OnDeath?.Invoke();
            }    

            OnHpChanged?.Invoke(this.currentHp, MaxHp);
        }
    }

    /// <summary> Entity�� ���� �̵� �������� �����Դϴ�. </summary>
    public virtual bool IsMoving
    {
        get => this.isMoving;
        set
        {
            if (this.isMoving != value)
            {
                this.isMoving = value;
                OnMoveStateChanged?.Invoke(value);
            }
        }
    }

    void Awake()
    {
        currentHp = MaxHp;
    }
}
