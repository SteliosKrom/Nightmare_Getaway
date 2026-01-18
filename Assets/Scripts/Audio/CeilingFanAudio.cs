using UnityEngine;

public class CeilingFanAudio : MonoBehaviour
{
    [Header("AUDIO")]
    public AudioLowPassFilter ceilingFanAudioLowPassFilter;
    public AudioSource ceilingFanAudioSource;
    public AudioClip ceilingFanAudioClip;
    public LayerMask groundLayer;

    private void Start()
    {
        ceilingFanAudioSource.clip = ceilingFanAudioClip;
        ceilingFanAudioSource.loop = true;
        AudioManager.Instance.StopSound(ceilingFanAudioSource);
    }

    private void Update()
    {
        if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (IsPlayerOnGround())
            {
                ceilingFanAudioSource.minDistance = 1f;
                ceilingFanAudioSource.maxDistance = 1.25f;
                ceilingFanAudioLowPassFilter.cutoffFrequency = Mathf.Lerp(ceilingFanAudioLowPassFilter.cutoffFrequency, 22000f, Time.deltaTime * 3f);
            }
            else
            {
                ceilingFanAudioSource.minDistance = 0.5f;
                ceilingFanAudioSource.maxDistance = 0.75f;
                ceilingFanAudioLowPassFilter.cutoffFrequency = Mathf.Lerp(ceilingFanAudioLowPassFilter.cutoffFrequency, 11000f, Time.deltaTime * 3f);
            }

            if (!ceilingFanAudioSource.isPlaying)
                AudioManager.Instance.PlaySFX(ceilingFanAudioSource, ceilingFanAudioClip);
        }
    }

    private bool IsPlayerOnGround()
    {
        return Physics.CheckSphere(transform.position, 0.1f, groundLayer);
    }
}
