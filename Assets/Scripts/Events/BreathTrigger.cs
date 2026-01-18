using UnityEngine;

public class BreathTrigger : MonoBehaviour
{
    [Header("AUDIO")]
    [SerializeField] private AudioSource breathAudioSource;
    [SerializeField] private AudioClip breathAudioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(breathAudioSource, breathAudioClip);
            gameObject.SetActive(false);
        }
    }
}
