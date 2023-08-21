using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Entity class의 Hert, Death, Attack 사운드를 제어합니다.
/// 만약 사운드가 인스펙터에서 등록되지 않더라도 오류를 일으키지 않습니다.
/// </summary>
[RequireComponent(typeof(Entity))]
public class EntitySoundplayer : MonoBehaviour
{
#nullable enable
    [SerializeField] private AudioClip? hertClip;
    [SerializeField] private AudioClip? deathClip;
    [SerializeField] private AudioClip? attackClip;
#nullable disable

    private Entity entity;
    private AudioSource entitySound;
    private AudioSource effectSound;

    void Awake()
    {
        entity = GetComponent<Entity>();

        // 입으로 소리내는 오디오 클립이 추가된 경우만 오디오 소스 컴포넌트를 추가합니다.
        if (hertClip != null || deathClip != null)
        {
            entitySound = this.gameObject.AddComponent<AudioSource>();
            entitySound.playOnAwake = false;
            // entitySound.clip의 default clip 초기화
            entitySound.clip = (hertClip != null ? hertClip : entitySound.clip);

            if (hertClip != null)
            {
                entity.OnHpChanged += OnHertSoundPlay;
            }
            if (deathClip != null)
            {
                entity.OnDeath += OnDeathSoundPlay;
            }
        }

        // 입이 아닌 다른곳에서 소리가 나는경우 동시에 소리가 재생될 수 있도록 합니다.
        if (attackClip != null)
        {
            effectSound = this.gameObject.AddComponent<AudioSource>();
            effectSound.playOnAwake = false;
            // entitySound.clip을 default clip 초기화
            effectSound.clip = (attackClip != null ? attackClip : entitySound.clip);
            entity.OnAttacked += OnAttackSoundPlay;
        }
    }

    /// <summary> Entity가 다쳤을 때 사운드 재생 </summary>
    private void OnHertSoundPlay(object sender, HpChangedEventArgs args)
    {
        // 체력이 감소할 때만 재생합니다.
        if (args.Increased == true) return;
        if (entitySound == null || hertClip == null) return;
        if (args.CurrentHp <= 0) return;

        entitySound.clip = hertClip;
        entitySound.Play();
    }

    /// <summary> Entity가 죽었을 때 사운드 재생 </summary>
    private void OnDeathSoundPlay(object sender, EventArgs args)
    {
        if (entitySound == null || deathClip == null) return;

        entitySound.clip = deathClip;
        entitySound.Play();
    }

    /// <summary> Entity가 공격했을 때 사운드 재생 </summary>
    private void OnAttackSoundPlay(object sender, EventArgs args)
    {
        if (effectSound == null || attackClip == null) return;

        effectSound.clip = attackClip;
        effectSound.Play();
    }
}
