using UnityEngine;

public class RadioAudio : MonoBehaviour
{
    [SerializeField] private AudioLowPassFilter radioLowPassFilter;
    [SerializeField] private AudioSource radioAudioSource;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            radioLowPassFilter.cutoffFrequency = 22000f;
        }
    }
}
