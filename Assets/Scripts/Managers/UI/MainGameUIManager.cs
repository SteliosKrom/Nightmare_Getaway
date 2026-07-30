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
    private float noteInputMenuDelay = 0.15f;

    #region STATES
    [Header("GAME STATES")]
    [SerializeField] private bool resumed = false;
    #endregion

    #region SERVICES
    private SettingsUIManager settingsUIManager;
    private AudioManager audioManager;
    private GameManager gameManager;
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

    [Header("CAMERA")]
    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private GameObject secondaryCameraObj;
    #endregion

    private void Start()
    {
        settingsUIManager = ServiceManager.GetService<SettingsUIManager>();
        gameManager = ServiceManager.GetService<GameManager>();
    }

    private void Update()
    {
        if (resumed)
        {
            if (audioManager.HeartbeatAudioSource.isPlaying)
            {
                audioManager.MainGameAudioSource.volume =
                    Mathf.Lerp(audioManager.MainGameAudioSource.volume, 0.025f, 2f * Time.deltaTime);
            }
        }
        NoteMenuInput();
    }

    public void NoteMenuInput()
    {
        if (gameManager.CurrentMenuState == MenuState.OnNoteMenu)
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

        audioManager.UnPauseSound(audioManager.MainGameAudioSource);
        audioManager.UnPauseSounds();

        resumeButton.transform.DOScale(0.8f, 0.2f);
        pauseManager.CheckDoorStateOnResume();
        Time.timeScale = 1.0f;

        DisableRedColorTextFromPauseButtons();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        resumed = true;

        gameManager.CurrentGameState = GameState.OnPlaying;
        gameManager.CurrentMenuState = MenuState.None;
    }

    public void SettingsButton()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);

        settingsUIManager.GetBackToPauseMenu.SetActive(true);
        settingsUIManager.SettingsMenu.SetActive(true);

        DisableRedColorTextFromPauseButtons();

        settingsButton.transform.DOScale(0.8f, 0.2f);
        gameManager.CurrentMenuState = MenuState.OnGameSettings;
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("MainGameScene");
        audioManager.Play(audioManager.MainMenuAudioSource);

        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        mainCameraObj.SetActive(false);
        secondaryCameraObj.SetActive(true);

        playerRespawn.Respawn();

        DisableRedColorTextFromPauseButtons();

        Time.timeScale = 1f;
        gameManager.CurrentMenuState = MenuState.OnMainMenu;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void DisableRedColorTextFromPauseButtons()
    {
        foreach (TextMeshProUGUI text in pauseButtonText)
            text.color = Color.white;
    }

    public IEnumerator NoteMenuInputDelay()
    {
        yield return new WaitForSeconds(noteInputMenuDelay);

        gameManager.CurrentGameState = GameState.OnPlaying;
        gameManager.CurrentMenuState = MenuState.None;

        interactor.NoteMenu.SetActive(false);
    }
}