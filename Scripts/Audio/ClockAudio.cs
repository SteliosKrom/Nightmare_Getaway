using UnityEngine;

public class ClockAudio : MonoBehaviour
{
    [Header("AUDIO")]
    public AudioSource clockAudioSource;
    public AudioLowPassFilter clockAudioLowPassFilter;
    public AudioClip clockAudioClip;
    public LayerMask groundLayer;

    private void Start()
    {
        AudioManager.Instance.StopSound(clockAudioSource);
    }

    private void Update()
    {
        if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            if (IsPlayerOnGround())
            {
                clockAudioSource.minDistance = 1f;
                clockAudioSource.maxDistance = 1.25f;
                clockAudioLowPassFilter.cutoffFrequency = Mathf.Lerp(clockAudioLowPassFilter.cutoffFrequency, 22000f, Time.deltaTime * 3f);
            }
            else
            {
                clockAudioSource.minDistance = 0.5f;
                clockAudioSource.maxDistance = 0.75f;
                clockAudioLowPassFilter.cutoffFrequency = Mathf.Lerp(clockAudioLowPassFilter.cutoffFrequency, 2000f, Time.deltaTime * 3f);
            }

            if (!clockAudioSource.isPlaying)
                AudioManager.Instance.PlaySFX(clockAudioSource, clockAudioClip);
        }
    }

    private bool IsPlayerOnGround()
    {
        return Physics.CheckSphere(transform.position, 0.1f, groundLayer);
    }
}
