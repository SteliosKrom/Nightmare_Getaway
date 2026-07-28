using UnityEngine;

public class DemonCryTrigger : MonoBehaviour
{
    #region SERVICES
    private AudioManager audioManager;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private Collider demonCryCollider;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource demonWhispersAudioSource;
    [SerializeField] private AudioClip demonWhispersAudioClip;
    #endregion

    private void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            demonCryCollider.enabled = false; 
            audioManager.PlaySFX(demonWhispersAudioSource, demonWhispersAudioClip);
        }
    }
}
