using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private float pauseDelay = 0.5f;

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private Interactor interactor;
    [SerializeField] private AddEventTrigger addEventTrigger;
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

    private void Update()
    {
        CheckIfResumed();
        PauseAndResume();
    }

    public void PauseAndResume()
    {
        KeyCode pause = KeybindManager.Instance.ActualKeybinds["Pause"];

        if (RoundManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu)
            return;

        if (RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu)
            return;

        if (KeybindManager.Instance.IsWaitingForKey == true)
            return;

        if (!Input.GetKeyDown(pause))
            return;

        if (!canPause)
            return;

        StartCoroutine(PauseDelay());
    }

    public void UpdateCursorDisplay()
    {
        if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            PauseGame();
        }
        else if (RoundManager.Instance.CurrentGameState == GameState.OnPause)
        {
            switch (RoundManager.Instance.CurrentMenuState)
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
            if (AudioManager.Instance.HeartbeatAudioSource.isPlaying)
            {
                AudioManager.Instance.MainGameAudioSource.volume = Mathf.Lerp(AudioManager.Instance.MainGameAudioSource.volume, 0.025f, 2f * Time.deltaTime);
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        interactor.LockedMessagePanel.SetActive(false);

        HUD.Instance.DisableAllHUDIcons();

        AudioManager.Instance.PauseSound(AudioManager.Instance.MainGameAudioSource);
        AudioManager.Instance.PauseSounds();

        CheckDoorStateOnPause();
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RoundManager.Instance.CurrentGameState = GameState.OnPause;
        RoundManager.Instance.CurrentMenuState = MenuState.OnPauseMenu;
    }

    public void ResumeGameFromGameSettings()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);

        SettingsUIManager.Instance.GetBackToPauseMenu.SetActive(false);

        HUD.Instance.DotIcon.SetActive(false);

        addEventTrigger.ExitHoverSoundEffectSettings(audioCategoryButton.transform);
        addEventTrigger.ExitHoverSoundEffectSettings(videoCategoryButton.transform);
        addEventTrigger.ExitHoverSoundEffectSettings(graphicsCategoryButton.transform);
        addEventTrigger.ExitHoverSoundEffectSettings(controlsCategoryButon.transform);

        addEventTrigger.ExitHoverEffectOther(SettingsUIManager.Instance.GetBackToSettingsFromGameButton.transform);
        addEventTrigger.ExitHoverEffectOther(SettingsUIManager.Instance.GetBackToSettingsButton.transform);

        ChangeButtonTextColor();

        Time.timeScale = 0f;
        RoundManager.Instance.CurrentGameState = GameState.OnPause;
        RoundManager.Instance.CurrentMenuState = MenuState.OnPauseMenu;
    }

    public void ResumeGameFromCategorySettings()
    {
        settingsMenu.SetActive(true);

        SettingsUIManager.Instance.GetBackToSettingsFromGame.SetActive(false);
        SettingsUIManager.Instance.GetBackToPauseMenu.SetActive(true);

        SettingsUIManager.Instance.HideAllCategories();
        ChangeButtonTextColor();

        addEventTrigger.ExitHoverEffectOther(SettingsUIManager.Instance.GetBackToSettingsFromGameButton.transform);
        RoundManager.Instance.CurrentMenuState = MenuState.OnGameSettings;
    }

    public void ResumeGameFromPauseMenu()
    {
        HUD.Instance.ShowDotOnly();
        pauseMenu.SetActive(false);

        AudioManager.Instance.UnPauseSound(AudioManager.Instance.MainGameAudioSource);
        AudioManager.Instance.UnPauseSounds();

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
        RoundManager.Instance.CurrentGameState = GameState.OnPlaying;
        RoundManager.Instance.CurrentMenuState = MenuState.None;
    }

    public void CheckDoorStateOnPause()
    {
        if (doorOpenedAudioSource.isPlaying)
        {
            AudioManager.Instance.PauseSound(doorOpenedAudioSource);
            isDoorOpenedSoundPaused = true;
        }
        else if (doorClosedAudioSource.isPlaying)
        {
            AudioManager.Instance.PauseSound(doorClosedAudioSource);
            isDoorClosedSoundPaused = true;
        }
    }

    public void CheckDoorStateOnResume()
    {
        if (isDoorOpenedSoundPaused)
        {
            AudioManager.Instance.UnPauseSound(doorOpenedAudioSource);
            isDoorOpenedSoundPaused = false;
        }
        else if (isDoorClosedSoundPaused)
        {
            AudioManager.Instance.UnPauseSound(doorClosedAudioSource);
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
        {
            text.color = Color.white;
        }
    }
}
