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

    /// <summary> Entity의 이동상태 변경시 이벤트 </summary>
    public event MoveStateChangedHandler OnMoveStateChanged;
    /// <summary> Entity의 체력 변경시 이벤트 </summary>
    public event HpChangedHandler OnHpChanged;
    /// <summary> Entity가 사망시 이벤트 </summary>
    public event DeathHandler OnDeath;

    [field: SerializeField] public float MaxHp { get; set; } = 100f;
    [field: SerializeField] public float Damage { get; set; } = 20f;

    private float currentHp;
    private bool isMoving;

    /// <summary> Entity가 현재 살아있는지의 여부입니다. </summary>
    public bool IsAlive => currentHp > 0f;

    /// <summary> 현재 Entity의 체력입니다. </summary>
    public float CurrentHp
    {
        get => this.currentHp;
        set
        {
            // 같은 값을 대입하려고 하거나, 체력을 0보다 적게 설정할 수 없습니다.
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

    /// <summary> Entity가 현재 이동 중인지의 여부입니다. </summary>
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
