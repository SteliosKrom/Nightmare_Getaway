using System.Collections;
using UnityEngine;

public class DollMovement : MonoBehaviour
{
    #region CHARACTERS
    [Header("CHARACTERS")]
    [SerializeField] private CharacterController dollCharacterController;
    #endregion

    #region GENERAL
    private float dollMovementDuration = 3;
    private float dollMovementSpeed = 7.5f;

    private bool hasTriggered = false;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private TriggerFlickering triggerFlickering;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject doll;
    #endregion

    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource dollFootstepsAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip dollFootstepsAudioClip;
    #endregion

    private void Update()
    {
        if (!hasTriggered && triggerFlickering.IsTriggered)
        {
            hasTriggered = true;
            MoveDoll();
        }
    }

    public void MoveDoll()
    {
        StartCoroutine(MoveDollDelay());
    }

    private IEnumerator MoveDollDelay()
    {
        float timer = 0f;
        float footstepsTimer = 0f;
        float stepInterval = 0.25f;

        while (timer < dollMovementDuration)
        {
            dollCharacterController.Move(Vector3.back * Time.deltaTime * dollMovementSpeed);
            footstepsTimer += Time.deltaTime;

            if (footstepsTimer >= stepInterval)
            {
                AudioManager.Instance.PlaySFX(dollFootstepsAudioSource, dollFootstepsAudioClip);
                dollFootstepsAudioSource.pitch = Random.Range(0.25f, 0.75f);
                footstepsTimer = 0f;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        doll.SetActive(false);
    }
}
