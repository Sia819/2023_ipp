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

        // ������ �Ҹ����� ����� Ŭ���� �߰��� ��츸 ����� �ҽ� ������Ʈ�� �߰��մϴ�.
        if (hertClip != null || deathClip != null)
        {
            entitySound = this.gameObject.AddComponent<AudioSource>();
            entitySound.playOnAwake = false;

            entitySound.clip = (hertClip != null ? hertClip : entitySound.clip);    // entitySound.clip�� �ʱ�ȭ

            if (hertClip != null)
            {
                entity.OnHpChanged += OnHertSoundPlay;
            }
            if (deathClip != null)
            {
                entity.OnDeath += OnDeathSoundPlay;
            }
        }

        // ���� �ƴ� �ٸ������� �Ҹ��� ���°�� ���ÿ� �Ҹ��� ����� �� �ֵ��� �մϴ�.
        if (deathClip != null)
        {
            effectSound = this.gameObject.AddComponent<AudioSource>();
            effectSound.playOnAwake = false;
            effectSound.clip = (deathClip != null ? deathClip : entitySound.clip);  // entitySound.clip�� �ʱ�ȭ
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
