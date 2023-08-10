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
        // 입으로 소리내는 오디오 클립이 추가된 경우만 오디오 소스 컴포넌트를 추가합니다.
        if (hitClip != null || deathClip != null)
        {
            entitySound = this.gameObject.AddComponent<AudioSource>();
            entitySound.playOnAwake = false;

            effectSound.clip = hitClip != null ? hitClip : effectSound.clip;    // effectSound.clip를 hitClip으로 초기화 합니다.
        }

        // 입이 아닌 다른곳에서 소리가 나는경우 동시에 소리가 재생될 수 있도록 합니다.
        if (deathClip != null)
        {
            effectSound = this.gameObject.AddComponent<AudioSource>();
            effectSound.playOnAwake = false;
        }
    }

}
