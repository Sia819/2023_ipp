using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region CustomEventArgs Class

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

public class AttackEventArgs : EventArgs
{
    private Vector3 _hitPoint;

    public Entity TargetEntity { get; }
    public float Damage { get; }
    public Vector3? HitPoint
    {
        get
        {
            if (TargetEntity == null) return null;
            else return _hitPoint;
        }
        private set
        {
            if (value != null)
                _hitPoint = value.Value;
        }
    }

    public AttackEventArgs(Entity targetEntity, float damage, Vector3? hitPoint)
    {
        TargetEntity = targetEntity;
        Damage = damage;
        HitPoint = hitPoint;
    }
}
#endregion

public abstract class Entity : MonoBehaviour
{
    public delegate void MoveStateChangedHandler(object sender, MoveStateEventArgs args);
    public delegate void HpChangedHandler(object sender, HpChangedEventArgs args);
    public delegate void DeathHandler(object sender, EventArgs args);
    public delegate void AttackedHandler(object sender, AttackEventArgs args);

    /// <summary> Entity의 이동상태 변경 시 이벤트 발생 </summary>
    public event MoveStateChangedHandler OnMoveStateChanged;
    /// <summary> Entity가 공격 시 이벤트 발생 </summary>
    public event AttackedHandler OnAttacked;
    /// <summary> Entity의 체력 변경되었을 때 이벤트 발생 </summary>
    public event HpChangedHandler OnHpChanged;
    /// <summary> Entity가 사망 시 이벤트 발생 </summary>
    public event DeathHandler OnDeath;

    [field: SerializeField] public float MaxHp { get; set; } = 200f;
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

            bool increased = (currentHp < value) ? true : false;

            if (value > 0)
                this.currentHp = value;
            else
            {
                this.currentHp = 0f;
                OnDeath?.Invoke(this, EventArgs.Empty);
            }
            
            OnHpChanged?.Invoke(this, new HpChangedEventArgs(this.currentHp, MaxHp, increased));
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

    /// <summary> 이 Entity가 공격을 했으며, <see cref="OnAttacked"/> 이벤트가 호출됩니다. </summary>
    public void ExcuteAttack(Entity targetEntity, float damage, Vector3? hitPoint)
    {
        OnAttacked?.Invoke(this, new AttackEventArgs(targetEntity, damage, hitPoint));
    }
}
