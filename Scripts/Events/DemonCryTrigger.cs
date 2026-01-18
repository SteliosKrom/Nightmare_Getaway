using UnityEngine;

public class DemonCryTrigger : MonoBehaviour
{
    [Header("COLLIDERS")]
    [SerializeField] private Collider demonCryCollider;

    [Header("AUDIO")]
    [SerializeField] private AudioSource demonWhispersAudioSource;
    [SerializeField] private AudioClip demonWhispersAudioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            demonCryCollider.enabled = false; 
            AudioManager.Instance.PlaySFX(demonWhispersAudioSource, demonWhispersAudioClip);
        }
    }
}
