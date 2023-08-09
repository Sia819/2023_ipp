using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public delegate void MoveStateChangedHandler(bool moving);
    public delegate void HpChangedHandler(float currentHp, float maxHp);
    public delegate void DeathHandler();

    /// <summary> Entity의 이동 상태 변경 시 이벤트 </summary>
    public event MoveStateChangedHandler OnMoveStateChanged;
    /// <summary> Entity의 체력 변경 시 이벤트 </summary>
    public event HpChangedHandler OnHpChanged;
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
                OnDeath?.Invoke();
            }    

            OnHpChanged?.Invoke(this.currentHp, MaxHp);
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
                OnMoveStateChanged?.Invoke(value);
            }
        }
    }

    void Awake()
    {
        currentHp = MaxHp;
    }
}
