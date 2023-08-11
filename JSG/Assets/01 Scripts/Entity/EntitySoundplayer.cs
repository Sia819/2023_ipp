using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Entity))]
public class EntitySoundplayer : MonoBehaviour
{
#nullable enable
    [SerializeField] private AudioClip? hertClip;
    [SerializeField] private AudioClip? deathClip;
    [SerializeField] private AudioClip? attackClip;
#nullable restore

    private Entity entity;
    private AudioSource entitySound;
    private AudioSource effectSound;

    void Start()
    {
        entity = GetComponent<Entity>();

        // 입으로 소리내는 오디오 클립이 추가된 경우만 오디오 소스 컴포넌트를 추가합니다.
        if (hertClip != null || deathClip != null)
        {
            entitySound = this.gameObject.AddComponent<AudioSource>();
            entitySound.playOnAwake = false;

            entitySound.clip = (hertClip != null ? hertClip : entitySound.clip);    // entitySound.clip을 초기화

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
        if (deathClip != null)
        {
            effectSound = this.gameObject.AddComponent<AudioSource>();
            effectSound.playOnAwake = false;
            effectSound.clip = (deathClip != null ? deathClip : entitySound.clip);  // entitySound.clip을 초기화
        }
    }

    private void OnHertSoundPlay(object sender, EventArgs args)
    {
        if (entitySound == null || hertClip == null) return;
        
        entitySound.clip = hertClip;
        entitySound.Play();
    }

    private void OnDeathSoundPlay(object sender, EventArgs args)
    {
        if (entitySound == null || deathClip == null) return;

        entitySound.clip = deathClip;
        entitySound.Play();
    }

    private void OnAttackSoundPlay(object sender, EventArgs args)
    {
        if (effectSound == null || attackClip == null) return;

        effectSound.clip = attackClip;
        effectSound.Play();
    }
}
