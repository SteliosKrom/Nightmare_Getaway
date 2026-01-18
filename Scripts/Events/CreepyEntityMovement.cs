using System.Collections;
using UnityEngine;

public class CreepyEntityMovement : MonoBehaviour
{
    #region GENERAL
    [Header("GENERAL")]
    [SerializeField] private CharacterController creepyEntityCharacterController;
    [SerializeField] private CharacterController playerCharacterController;

    private float creepyEntitySpeed = 2.5f;
    private float creepyEntityMovementDuration = 5f;

    private bool hasTriggered = false;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Interactor interactor;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject creepyEntity;
    [SerializeField] private GameObject creepyEntityTrigger;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider creepyEntityTriggerObstacle;
    [SerializeField] private BoxCollider creepyEntityTriggerCollider;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource creepyEntityFootstepsAudioSource;
    [SerializeField] private AudioSource creepyEntityStressSFX;
    [SerializeField] private AudioSource heartbeatGameAudioSource;

    [SerializeField] private AudioClip creepyEntityFootstepsAudioClip;
    [SerializeField] private AudioClip creepyEntityStressSFXClip;
    #endregion

    #region LIGHT
    [Header("LIGHT")]
    [SerializeField] private Light flashlight;

    private float minIntensity = 0f;
    private float maxIntensity = 25f;
    private float flickerSpeed = 0.05f;
    private float flickerDelay = 5f;
    #endregion

    private void Update()
    {
        if (hasTriggered) return;

        bool playerInsideTrigger = playerCharacterController.bounds.Intersects(creepyEntityTriggerCollider.bounds);

        if (playerInsideTrigger && interactor.HasFlashlight && interactor.CanToggle)
        {
            hasTriggered = true;
            interactor.CanToggle = false;

            AudioManager.Instance.StopSound(heartbeatGameAudioSource);
            AudioManager.Instance.PlaySFX(creepyEntityStressSFX, creepyEntityStressSFXClip);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.FlashlightFlicker.source, AudioManager.Instance.FlashlightFlicker.clip);

            InvokeRepeating("TriggerFlicker", 0f, flickerSpeed);
            MoveEntity();
        }
    }

    private void MoveEntity()
    {
        StartCoroutine(CreepyEntityMovementDelayCoroutine());
    }

    private void TriggerFlicker()
    {
        StartCoroutine(TriggerFlickerDelay());
    }

    private IEnumerator TriggerFlickerDelay()
    {
        flashlight.intensity = Random.Range(minIntensity, maxIntensity);
        flashlight.enabled = true;
        AudioManager.Instance.StopSound(heartbeatGameAudioSource);

        yield return new WaitForSeconds(flickerDelay);

        flashlight.intensity = 75f;
        CancelInvoke();
        AudioManager.Instance.MainGameAudioSource.volume = Mathf.Lerp(AudioManager.Instance.MainGameAudioSource.volume, 0.1f, 2f * Time.deltaTime);
        interactor.CanToggle = true;
    }

    private IEnumerator CreepyEntityMovementDelayCoroutine()
    {
        float timer = 0f;
        float footstepsTimer = 0f;
        float stepInterval = 0.75f;

        while (timer < creepyEntityMovementDuration)
        {
            creepyEntityCharacterController.Move(Vector3.forward * creepyEntitySpeed * Time.deltaTime);
            footstepsTimer += Time.deltaTime;

            if (footstepsTimer >= stepInterval)
            {
                AudioManager.Instance.PlaySFX(creepyEntityFootstepsAudioSource, creepyEntityFootstepsAudioClip);
                creepyEntityFootstepsAudioSource.pitch = Random.Range(0.25f, 0.75f);
                footstepsTimer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        creepyEntityTriggerObstacle.enabled = false;
        creepyEntity.SetActive(false);
        creepyEntityTriggerCollider.enabled = false;
    }
}
