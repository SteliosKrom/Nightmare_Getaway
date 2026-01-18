using UnityEngine;

public class TelephoneAudio : MonoBehaviour
{
    [SerializeField] private AudioSource telephoneAudioSource;
    [SerializeField] private AudioLowPassFilter telephoneAudioLowPassFilter;

    private void OnCollisionEnter(Collision other)
    { 
        if (other.gameObject.CompareTag("Ground"))
        {
            telephoneAudioSource.Play();
            telephoneAudioLowPassFilter.cutoffFrequency = 22000;
        }
    }
}
