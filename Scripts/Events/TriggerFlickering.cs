using System.Collections;
using UnityEngine;

public class TriggerFlickering : MonoBehaviour
{
    #region GENERAL
    private float minIntensity = 0f;
    private float maxIntensity = 75f;
    private float flickerSpeed = 0.05f;
    private float flickerDelay = 5f;

    private float demonDollTriggerObstacleDelay = 3f;

    private bool isTriggered = false;
    private bool isFlickering = false;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Interactor interactor;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject demonDollTriggerObstacle;
    #endregion

    #region AUDIO 
    [Header("AUDIO")]
    [SerializeField] private AudioSource creepyLaughAudioSource;
    [SerializeField] private AudioClip creepyLaughAudioClip;
    #endregion

    #region COLLIDERS
    [Header("COLLIDERS")]
    [SerializeField] private BoxCollider onTrigger;
    #endregion

    [Header("OTHER")]
    public Light newLight;

    public bool IsTriggered { get { return isTriggered; } set { isTriggered = value; } }
    public bool IsFlickering { get { return isFlickering; } }

    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag("Player") && interactor.HasFlashlight && interactor.CanToggle)
        {
            IsTriggered = true;
            isFlickering = true;
            onTrigger.enabled = false;
            AudioManager.Instance.PlaySFX(creepyLaughAudioSource, creepyLaughAudioClip);
            StartCoroutine(FlickerDelay());
            StartCoroutine(DemonDollObstacleLifetimeDelay());
        }
    }

    public void ToggleFlicker()
    {
        bool isPlaying = RoundManager.Instance.CurrentGameState == GameState.OnPlaying;

        if (isPlaying)
        {
            if (isFlickering)
            {
                float randomIntensity = Random.Range(minIntensity, maxIntensity);
                newLight.intensity = randomIntensity;
            }
        }
    }

    private IEnumerator FlickerDelay()
    {
        InvokeRepeating("ToggleFlicker", 0f, flickerSpeed);

        yield return new WaitForSeconds(flickerDelay);

        CancelInvoke();
        isFlickering = false;
        newLight.intensity = 75f;
    }

    private IEnumerator DemonDollObstacleLifetimeDelay()
    {
        demonDollTriggerObstacle.SetActive(true);
        yield return new WaitForSeconds(demonDollTriggerObstacleDelay);
        demonDollTriggerObstacle.SetActive(false);
    }
}
