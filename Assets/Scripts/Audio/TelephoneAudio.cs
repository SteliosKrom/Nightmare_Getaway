using UnityEngine;

public class TelephoneAudio : MonoBehaviour
{
    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource telephoneAudioSource;
    [SerializeField] private AudioLowPassFilter telephoneAudioLowPassFilter;
    #endregion

    private void OnCollisionEnter(Collision other)
    { 
        if (other.gameObject.CompareTag("Ground"))
        {
            telephoneAudioSource.Play();
            telephoneAudioLowPassFilter.cutoffFrequency = 22000;
        }
    }
}
