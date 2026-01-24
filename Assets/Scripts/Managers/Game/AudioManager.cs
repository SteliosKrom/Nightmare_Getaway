using Jemeza.SFASFX;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private GameObject triggerInteractable3DAudio;

    [System.Serializable]
    public struct AudioItem
    {
        public AudioSource source;
        public AudioClip clip;
    }

    [SerializeField] private AudioItem flashlightFlicker;
    [SerializeField] private AudioItem doorOpened;
    [SerializeField] private AudioItem doorClosed;
    [SerializeField] private AudioItem lockedDoor;
    [SerializeField] private AudioItem hover;
    [SerializeField] private AudioItem ligthSwitches;
    [SerializeField] private AudioItem equipItem;
    [SerializeField] private AudioItem placeItem;
    [SerializeField] private AudioItem collectNote;

    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource[] audioSources;
    [SerializeField] private AudioSource mainMenuAudioSource;
    [SerializeField] private AudioSource mainGameAudioSource;
    [SerializeField] private AudioSource heartbeatAudioSource;
    #endregion

    public AudioSource MainMenuAudioSource => mainMenuAudioSource;
    public AudioSource MainGameAudioSource => mainGameAudioSource;
    public AudioSource HeartbeatAudioSource => heartbeatAudioSource;

    public AudioItem FlashlightFlicker => flashlightFlicker; public AudioItem LockedDoor => lockedDoor;
    public AudioItem Hover => hover; public AudioItem LightSwitches => ligthSwitches;
    public AudioItem EquipItem => equipItem; public AudioItem PlaceItem => placeItem;
    public AudioItem CollectNote => collectNote; public AudioItem DoorOpened => doorOpened;
    public AudioItem DoorClosed => doorClosed;

    public GameObject TriggerInteractable3DMusic => triggerInteractable3DAudio;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("The instance of the object already exists");
        }
    }

    public void PlaySFX(AudioSource source, AudioClip clip) { source.PlayOneShot(clip); }
    public void Play(AudioSource source) { source.Play(); }
    public void StopSound(AudioSource source) { source.Stop(); }
    public void PauseSound(AudioSource source) { source.Pause(); }
    public void UnPauseSound(AudioSource source) { source.UnPause(); }
    public void PauseSounds()
    {
        foreach (AudioSource source in audioSources)
        {
            source.Pause();
        }
    }
    public void UnPauseSounds()
    {
        foreach (AudioSource source in audioSources)
        {
            source.UnPause();
        }
    }

    // Main menu music
    public void PlayMainMenuMusic() { mainMenuAudioSource.Play(); }
    public void StopMainMenuMusic() { mainMenuAudioSource.Stop(); }

    // Main game music
    public void PlayMainGameMusic() { mainGameAudioSource.Play(); }
    public void StopMainGameMusic() { mainGameAudioSource.Stop(); }

    // Hearbeat game music 
    public void PlayHeartbeatGameMusic() { heartbeatAudioSource.Play(); }
    public void StopHeartbeatGameMusic() { heartbeatAudioSource.Stop(); }
}
