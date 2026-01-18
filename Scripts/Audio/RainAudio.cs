using UnityEngine;

public class RainAudio : MonoBehaviour
{
    private float targetFogDensityOutdoors = 0.01f;
    private float targetFogDensityIndoors = 0.005f;

    [Header("AUDIO")]
    public AudioSource rainAudioSource;
    public AudioLowPassFilter rainLowPassFilter;

    private void Start()
    {
        rainLowPassFilter.cutoffFrequency = 22000f;
    }

    private void Update()
    {
        if (RoundManager.Instance.CurrentEnvironmentState == EnvironmentState.OnOutdoors)
        {
            if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
            {
                rainAudioSource.minDistance = 2.5f;
                rainAudioSource.maxDistance = 5f;
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensityOutdoors, Time.deltaTime * 3f);
                rainLowPassFilter.cutoffFrequency = Mathf.Lerp(rainLowPassFilter.cutoffFrequency, 22000f, Time.deltaTime * 3f);
            }
        }
        else if (RoundManager.Instance.CurrentEnvironmentState == EnvironmentState.OnIndoors)
        {
            if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
            {
                rainAudioSource.minDistance = 1f;
                rainAudioSource.maxDistance = 2.5f;
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetFogDensityIndoors, Time.deltaTime * 3f);
                rainLowPassFilter.cutoffFrequency = Mathf.Lerp(rainLowPassFilter.cutoffFrequency, 500f, Time.deltaTime * 3f);
            }     
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Indoor"))
            {
                RoundManager.Instance.CurrentEnvironmentState = EnvironmentState.OnIndoors;
            }
            else if (gameObject.CompareTag("Outdoor"))
            {
                RoundManager.Instance.CurrentEnvironmentState = EnvironmentState.OnOutdoors;
            }
        }
    }
}
