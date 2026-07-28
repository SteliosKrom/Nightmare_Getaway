using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private float pauseDelay = 0.15f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Interactor interactor;
    [SerializeField] private AddEventTrigger addEventTrigger;
    #endregion

    #region SERVICES
    private SettingsUIManager settingsUIManager;
    private GameManager gameManager;
    private AudioManager audioManager;
    private KeybindManager keybindManager;
    #endregion

    #region STATES
    [Header("GAME STATES")]
    private bool isDoorOpenedSoundPaused = false;
    private bool isDoorClosedSoundPaused = false;
    private bool canPause = true;
    private bool resumed = false;
    #endregion

    #region MAIN GAME
    [Header("MAIN GAME")]
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Button exitButton;
    #endregion

    #region SETTINGS MENU
    [Header("SETTINGS MENU")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject controlsMenu;
    #endregion

    #region BUTTONS
    [Header("BUTTONS")]
    [SerializeField] private Button audioCategoryButton;
    [SerializeField] private Button videoCategoryButton;
    [SerializeField] private Button graphicsCategoryButton;
    [SerializeField] private Button controlsCategoryButon;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioSource doorClosedAudioSource;
    [SerializeField] private AudioSource doorOpenedAudioSource;
    [SerializeField] private AudioSource inventoryAudioSource;
    #endregion

    #region TEXT
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI[] allButtonTexts;
    #endregion

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
        audioManager = ServiceManager.GetService<AudioManager>();
        keybindManager = ServiceManager.GetService<KeybindManager>();
    }

    private void Update()
    {
        CheckIfResumed();
        PauseAndResume();
    }

    public void PauseAndResume()
    {
        KeyCode pause = keybindManager.ActualKeybinds["Pause"];

        if (gameManager.CurrentMenuState == MenuState.OnInventoryMenu ||
            gameManager.CurrentMenuState == MenuState.OnNoteMenu)
            return;

        if (keybindManager.IsWaitingForKey == true)
            return;

        if (!Input.GetKeyDown(pause))
            return;

        if (!canPause)
            return;

        StartCoroutine(PauseDelay());
    }

    public void UpdateCursorDisplay()
    {
        if (gameManager.CurrentGameState == GameState.OnPlaying)
        {
            PauseGame();
        }
        else if (gameManager.CurrentGameState == GameState.OnPause)
        {
            switch (gameManager.CurrentMenuState)
            {
                case MenuState.OnPauseMenu:
                    ResumeGameFromPauseMenu();
                    break;
                case MenuState.OnGameSettings:
                    ResumeGameFromGameSettings();
                    break;
                case MenuState.OnCategorySettings:
                    ResumeGameFromCategorySettings();
                    break;
            }
        }
    }

    public void CheckIfResumed()
    {
        if (resumed)
        {
            if (audioManager.HeartbeatAudioSource.isPlaying)
            {
                audioManager.MainGameAudioSource.volume =
                    Mathf.Lerp(audioManager.MainGameAudioSource.volume, 0.025f, 2f * Time.deltaTime);
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        interactor.LockedMessagePanel.SetActive(false);

        HUD.Instance.DisableAllHUDIcons();

        audioManager.PauseSound(audioManager.MainGameAudioSource);
        audioManager.PauseSounds();

        CheckDoorStateOnPause();
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameManager.CurrentGameState = GameState.OnPause;
        gameManager.CurrentMenuState = MenuState.OnPauseMenu;
    }

    public void ResumeGameFromGameSettings()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        settingsUIManager.GetBackToPauseMenu.SetActive(false);

        HUD.Instance.DotIcon.SetActive(false);

        addEventTrigger.ExitHoverSoundEffectSettings(audioCategoryButton.transform);
        addEventTrigger.ExitHoverSoundEffectSettings(videoCategoryButton.transform);
        addEventTrigger.ExitHoverSoundEffectSettings(graphicsCategoryButton.transform);
        addEventTrigger.ExitHoverSoundEffectSettings(controlsCategoryButon.transform);

        addEventTrigger.ExitHoverEffectOther(settingsUIManager.GetBackToSettingsFromGameButton.transform);
        addEventTrigger.ExitHoverEffectOther(settingsUIManager.GetBackToSettingsButton.transform);

        ChangeButtonTextColor();

        Time.timeScale = 0f;

        gameManager.CurrentGameState = GameState.OnPause;
        gameManager.CurrentMenuState = MenuState.OnPauseMenu;
    }

    public void ResumeGameFromCategorySettings()
    {
        settingsMenu.SetActive(true);

        settingsUIManager.GetBackToSettingsFromGame.SetActive(false);
        settingsUIManager.GetBackToPauseMenu.SetActive(true);

        settingsUIManager.HideAllCategories();
        ChangeButtonTextColor();

        addEventTrigger.ExitHoverEffectOther(settingsUIManager.GetBackToSettingsFromGameButton.transform);
        gameManager.CurrentMenuState = MenuState.OnGameSettings;
    }

    public void ResumeGameFromPauseMenu()
    {
        HUD.Instance.ShowDotOnly();
        pauseMenu.SetActive(false);

        audioManager.UnPauseSound(audioManager.MainGameAudioSource);
        audioManager.UnPauseSounds();

        ChangeButtonTextColor();

        addEventTrigger.ExitHoverEffectPause(resumeButton.transform);
        addEventTrigger.ExitHoverEffectPause(settingsButton.transform);
        addEventTrigger.ExitHoverEffectPause(homeButton.transform);
        addEventTrigger.ExitHoverEffectPause(exitButton.transform);

        CheckDoorStateOnResume();
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        resumed = true;

        gameManager.CurrentGameState = GameState.OnPlaying;
        gameManager.CurrentMenuState = MenuState.None;
    }

    public void CheckDoorStateOnPause()
    {
        if (doorOpenedAudioSource.isPlaying)
        {
            audioManager.PauseSound(doorOpenedAudioSource);
            isDoorOpenedSoundPaused = true;
        }
        else if (doorClosedAudioSource.isPlaying)
        {
            audioManager.PauseSound(doorClosedAudioSource);
            isDoorClosedSoundPaused = true;
        }
    }

    public void CheckDoorStateOnResume()
    {
        if (isDoorOpenedSoundPaused)
        {
            audioManager.UnPauseSound(doorOpenedAudioSource);
            isDoorOpenedSoundPaused = false;
        }
        else if (isDoorClosedSoundPaused)
        {
            audioManager.UnPauseSound(doorClosedAudioSource);
            isDoorClosedSoundPaused = false;
        }
    }

    public IEnumerator PauseDelay()
    {
        canPause = false;
        yield return new WaitForSecondsRealtime(pauseDelay);
        UpdateCursorDisplay();
        canPause = true;
    }

    public void ChangeButtonTextColor()
    {
        foreach (TextMeshProUGUI text in allButtonTexts)
            text.color = Color.white;
    }
}
