using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameUIManager : MonoBehaviour
{
    private float noteInputMenuDelay = 1f;

    #region STATES
    [Header("GAME STATES")]
    [SerializeField] private bool resumed = false;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Interactor interactor;
    [SerializeField] private PlayerRespawn playerRespawn;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private TaskManager taskManager;
    [SerializeField] private HUD headsUpDisplay;
    #endregion

    #region UI PANELS
    [Header("MENUS & PANELS")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider gammaSlider;
    [SerializeField] private TextMeshProUGUI[] pauseButtonText;
    [SerializeField] private Button[] itemButtons;
    #endregion

    #region GAME
    [Header("MAIN GAME UI")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    #endregion

    #region PAUSE
    [Header("PAUSE GAME")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button exitButton;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource[] audioSources;
    #endregion

    #region CAMERAS & POST-PROCESSING
    [Header("POST-PROCESSING")]
    [SerializeField] private Volume mainCameraVolume;
    [SerializeField] private Volume secondaryCameraVolume;
    private ColorAdjustments mainCameraColorAdjustments;
    private ColorAdjustments secondaryCameraColorAdjustments;

    [Header("CAMERA")]
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private GameObject secondaryCameraObj;
    #endregion

    private void Update()
    {
        if (resumed)
        {
            if (AudioManager.Instance.HeartbeatAudioSource.isPlaying)
            {
                AudioManager.Instance.MainGameAudioSource.volume = Mathf.Lerp(AudioManager.Instance.MainGameAudioSource.volume, 0.025f, 2f * Time.deltaTime);
            }
        }
        NoteMenuInput();
    }

    public void NoteMenuInput()
    {
        if (RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(NoteMenuInputDelay());
            }
        }
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        HUD.Instance.DotIcon.SetActive(true);

        AudioManager.Instance.UnPauseSound(AudioManager.Instance.MainGameAudioSource);

        AudioManager.Instance.UnPauseSounds();

        resumeButton.transform.DOScale(0.8f, 0.2f);
        pauseManager.CheckDoorStateOnResume();
        Time.timeScale = 1.0f;

        DisableRedColorTextFromPauseButtons();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        resumed = true;
        RoundManager.Instance.CurrentGameState = GameState.OnPlaying;
        RoundManager.Instance.CurrentMenuState = MenuState.None;
    }

    public void SettingsButton()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        SettingsUIManager.Instance.GetBackToPauseMenu.SetActive(true);
        SettingsUIManager.Instance.SettingsMenu.SetActive(true);

        DisableRedColorTextFromPauseButtons();

        settingsButton.transform.DOScale(0.8f, 0.2f);
        RoundManager.Instance.CurrentMenuState = MenuState.OnGameSettings;
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("MainGameScene");
        AudioManager.Instance.Play(AudioManager.Instance.MainMenuAudioSource);

        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        mainCameraObj.SetActive(false);
        secondaryCameraObj.SetActive(true);
        playerRespawn.Respawn();

        DisableRedColorTextFromPauseButtons();

        Time.timeScale = 1f;
        RoundManager.Instance.CurrentMenuState = MenuState.OnMainMenu;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void DisableRedColorTextFromPauseButtons()
    {
        foreach (TextMeshProUGUI text in pauseButtonText)
        {
            text.color = Color.white;
        }
    }

    public IEnumerator NoteMenuInputDelay()
    {
        yield return new WaitForSeconds(noteInputMenuDelay);
        RoundManager.Instance.CurrentGameState = GameState.OnPlaying;
        interactor.NoteMenu.SetActive(false);
    }
}