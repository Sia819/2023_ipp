using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public delegate void MoveStateChangedHandler(bool moving);
    public delegate void HpChangedHandler(float currentHp);

    /// <summary> Entity�� �̵� ���� ���� �� �̺�Ʈ </summary>
    public event MoveStateChangedHandler OnMoveStateChanged;
    /// <summary> Entity�� ü�� ���� �� �̺�Ʈ </summary>
    public event HpChangedHandler OnHpChanged;

    public float maxHp = 100f;
    public float damage = 20f;

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
                Death();
            }    

            OnHpChanged?.Invoke(value);
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
        currentHp = maxHp;
    }

    internal abstract void Death();
}
