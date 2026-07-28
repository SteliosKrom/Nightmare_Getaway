using UnityEngine;

public class CeilingFanAudio : MonoBehaviour
{
    public LayerMask groundLayer;

    #region SERVICES
    private AudioManager audioManager;
    private GameManager gameManager;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    public AudioLowPassFilter ceilingFanAudioLowPassFilter;
    public AudioSource ceilingFanAudioSource;
    public AudioClip ceilingFanAudioClip;
    #endregion

    private void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();
        gameManager = ServiceManager.GetService<GameManager>();

        ceilingFanAudioSource.clip = ceilingFanAudioClip;
        ceilingFanAudioSource.loop = true;
        audioManager.StopSound(ceilingFanAudioSource);
    }

    private void Update()
    {
        if (gameManager.CurrentGameState == GameState.OnPlaying)
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
                audioManager.PlaySFX(ceilingFanAudioSource, ceilingFanAudioClip);
        }
    }

    private bool IsPlayerOnGround()
    {
        return Physics.CheckSphere(transform.position, 0.1f, groundLayer);
    }
}
