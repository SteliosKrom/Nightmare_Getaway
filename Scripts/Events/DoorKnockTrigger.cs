using UnityEngine;

public class DoorKnockTrigger : MonoBehaviour
{
    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource doorKnockAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip doorKnockAudioClip;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject doorKnockTrigger;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(doorKnockAudioSource, doorKnockAudioClip);
            doorKnockTrigger.SetActive(false);
        }
    }
}
