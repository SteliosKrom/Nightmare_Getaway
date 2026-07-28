using UnityEngine;

public class BreathTrigger : MonoBehaviour
{
    #region SERVICES
    private AudioManager audioManager;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource breathAudioSource;
    [SerializeField] private AudioClip breathAudioClip;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioManager.PlaySFX(breathAudioSource, breathAudioClip);
            gameObject.SetActive(false);
        }
    }
}
