using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Entity))]
public class EntitySoundplayer : MonoBehaviour
{
#nullable enable
    [SerializeField] private AudioClip? hitClip;
    [SerializeField] private AudioClip? deathClip;
    [SerializeField] private AudioClip? attackClip;
#nullable restore

    private AudioSource entitySound;
    private AudioSource effectSound;

    void Start()
    {
        // ������ �Ҹ����� ����� Ŭ���� �߰��� ��츸 ����� �ҽ� ������Ʈ�� �߰��մϴ�.
        if (hitClip != null || deathClip != null)
        {
            entitySound = this.gameObject.AddComponent<AudioSource>();
            entitySound.playOnAwake = false;

            effectSound.clip = hitClip != null ? hitClip : effectSound.clip;    // effectSound.clip�� hitClip���� �ʱ�ȭ �մϴ�.
        }

        // ���� �ƴ� �ٸ������� �Ҹ��� ���°�� ���ÿ� �Ҹ��� ����� �� �ֵ��� �մϴ�.
        if (deathClip != null)
        {
            effectSound = this.gameObject.AddComponent<AudioSource>();
            effectSound.playOnAwake = false;
        }
    }

}
