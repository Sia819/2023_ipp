using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    #region CustomEventArgs

    public class MoveStateEventArgs : EventArgs
    {
        public bool IsMoving { get; }

        public MoveStateEventArgs(bool isMoving)
        {
            IsMoving = isMoving;
        }
    }

    public class HpChangedEventArgs : EventArgs
    {
        public float CurrentHp { get; }
        public float MaxHp { get; }
        public bool Increased { get; }

        public HpChangedEventArgs(float currentHp, float maxHp, bool increased = false)
        {
            CurrentHp = currentHp;
            MaxHp = maxHp;
            Increased = increased;
        }
    }
    #endregion

    public delegate void MoveStateChangedHandler(object sender, MoveStateEventArgs args);
    public delegate void HpChangedHandler(object sender, HpChangedEventArgs args);
    public delegate void DeathHandler(object sender, EventArgs args);

    /// <summary> Entity�� �̵����� ����� �̺�Ʈ </summary>
    public event MoveStateChangedHandler OnMoveStateChanged;
    /// <summary> Entity�� ü�� ����� �̺�Ʈ </summary>
    public event HpChangedHandler OnHpChanged;
    /// <summary> Entity�� ����� �̺�Ʈ </summary>
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
                OnDeath?.Invoke(this, EventArgs.Empty);
            }

            OnHpChanged?.Invoke(this, new HpChangedEventArgs(this.currentHp, MaxHp));
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
                OnMoveStateChanged?.Invoke(this, new MoveStateEventArgs(this.isMoving));
            }
        }
    }

    void Awake()
    {
        currentHp = MaxHp;
    }
}
