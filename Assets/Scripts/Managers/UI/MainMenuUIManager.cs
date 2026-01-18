using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Runtime.CompilerServices;

public class MainMenuUIManager : MonoBehaviour
{
    #region GENERAL
    public bool fogForFirstTime = true;

    private readonly float mainMenuEntryDelay = 5f;
    private readonly float gameIntroDelay = 25f;
    private readonly float introTitleDescTextFadeOutDelay = 3f;
    private readonly float storyIntroTitleDelay = 5f;
    private readonly float endGameIntroDelay = 0.1f;

    private readonly string storyIntroTitleFullText = "Friday, December 13, 1998 – Blackridge, 12:53AM";
    private readonly string storyIntroFullText =
        "A storm raged over the empty town.\n" +
        "Ethan and Ryan thought it was just another adventure.\n\n" +
        "<b>Ryan tried to stop it—too late.</b>\n\n" +
        "Now Ethan is trapped in a place where nightmares breathe.\n" +
        "And the darkness may not only surround him… but live inside him.";
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private HUD HUD;
    [SerializeField] private TypewriterEffect typewriterEffect;
    [SerializeField] private OutdoorCameraEffect outdoorCameraEffect;
    #endregion

    #region TEXT
    [Header("TEXT ELEMENTS")]
    [SerializeField] private TextMeshProUGUI storyIntroText;
    [SerializeField] private TextMeshProUGUI storyIntroTitleText;
    [SerializeField] private TextMeshProUGUI backToCreditsMenuText;
    [SerializeField] private TextMeshProUGUI[] menuButtonsText;
    #endregion

    #region BUTTONS
    [Header("MAIN MENU BUTTONS")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backToMenuButtonCredits;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject gameIntroPanel;
    [SerializeField] private GameObject backToCreditsButton;
    #endregion

    #region AUDIO
    [Header("AUDIO SOURCES")]
    [SerializeField] private AudioSource typewriterAudioSource;
    [SerializeField] private AudioSource heartbeatGameAudioSource;
    [SerializeField] private AudioSource rainAudioSource;
    [SerializeField] private AudioSource menuEntryAudioSource;

    [Header("AUDIO CLIPS")]
    [SerializeField] private AudioClip typewriterAudioClip;
    [SerializeField] private AudioClip menuEntryAudioClip;
    #endregion

    #region CAMERAS & LIGHTING
    [Header("CAMERAS")]
    [SerializeField] private Camera mainCamera;

    [Header("LIGHTING")]
    [SerializeField] private Light kidRoomLight;
    [SerializeField] private Light outdoorLight;
    #endregion

    #region 
    [Header("ANIMATORS")]
    [SerializeField] private Animator introDescriptionAnimator;
    [SerializeField] private Animator introTextAnimator;
    [SerializeField] private Animator titleMenuAnimator;
    [SerializeField] private Animator pressAnyKeyAnimator;
    [SerializeField] private Animator pressSpaceToSkipIntroAnimator;
    #endregion

    public GameObject MainMenu => mainMenu;

    private void Start()
    {
        Time.timeScale = 1f;

        titleMenu.SetActive(true);

        AudioManager.Instance.Play(AudioManager.Instance.MainMenuAudioSource);
        AudioManager.Instance.Play(rainAudioSource);

        AudioManager.Instance.StopSound(AudioManager.Instance.MainGameAudioSource);
        AudioManager.Instance.StopSound(heartbeatGameAudioSource);
    }

    private void Update()
    {
        PressSpaceToSkipIntro();
        PressAnyKeyDown();
    }

    public void PressSpaceToSkipIntro()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (RoundManager.Instance.CurrentGameState == GameState.OnIntro)
            {
                EndGameIntro();
                StopAllCoroutines();
                kidRoomLight.enabled = true;
                RenderSettings.fogDensity = 0.005f;
                AudioManager.Instance.StopSound(typewriterAudioSource);
                StartCoroutine(AfterEndGameIntroDelay());
            }
        }
    }

    public void PressAnyKeyDown()
    {
        if (RoundManager.Instance.CurrentMenuState == MenuState.OnTitleMenu)
        {
            if (Input.anyKeyDown)
            {
                AudioManager.Instance.PlaySFX(menuEntryAudioSource, menuEntryAudioClip);

                titleMenuAnimator.SetBool("IsFadingOut", true);
                pressAnyKeyAnimator.SetBool("IsFadingOut", true);

                StartCoroutine(MainMenuEntryDelay());
                RoundManager.Instance.CurrentMenuState = MenuState.OnMainMenu;
            }
        }
    }

    public void EndGameIntro()
    {
        HUD.Instance.DotIcon.SetActive(true);

        Time.timeScale = 1f;

        outdoorCameraEffect.SecondaryCameraObj.SetActive(false);
        outdoorCameraEffect.MainCameraObj.SetActive(true);

        mainGame.SetActive(true);
        titleMenu.SetActive(false);
        mainMenu.SetActive(false);
        gameIntroPanel.SetActive(false);
        loadingPanel.SetActive(false);

        playButton.transform.DOScale(1f, 0.2f);
        RoundManager.Instance.CurrentGameState = GameState.OnPlaying;
    }

    public void PlayButton()
    {
        StartCoroutine(PlayButtonDelay());
    }

    private IEnumerator AfterEndGameIntroDelay()
    {
        yield return new WaitForSeconds(endGameIntroDelay);
        AudioManager.Instance.UnPauseSound(rainAudioSource);
        AudioManager.Instance.Play(AudioManager.Instance.MainGameAudioSource);
    }

    private IEnumerator MainMenuEntryDelay()
    {
        yield return new WaitForSeconds(mainMenuEntryDelay);
        mainMenu.SetActive(true);
    }

    public IEnumerator PlayButtonDelay()
    {
        RoundManager.Instance.CurrentMenuState = MenuState.None;
        RoundManager.Instance.CurrentEnvironmentState = EnvironmentState.OnIndoors;
        float playButtonDelay = Random.Range(1f, 2f); // Random delay between 1 to 2 seconds, change 10 to 20 later

        loadingPanel.SetActive(true);

        AudioManager.Instance.StopSound(AudioManager.Instance.MainMenuAudioSource);
        AudioManager.Instance.StopSound(menuEntryAudioSource);
        AudioManager.Instance.PauseSound(rainAudioSource);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        outdoorLight.enabled = false;

        yield return new WaitForSeconds(playButtonDelay);

        gameIntroPanel.SetActive(true);
        loadingPanel.SetActive(false);

        if (typewriterEffect.CoroutineIsRunning) yield break;
        StartCoroutine(typewriterEffect.PlayStoryIntroTitleTextTypeWriterDelay(storyIntroTitleText, storyIntroTitleFullText));

        yield return new WaitForSeconds(storyIntroTitleDelay);

        RoundManager.Instance.CurrentGameState = GameState.OnIntro;
        StartCoroutine(typewriterEffect.PlayStoryIntroTextTypeWriterDelay(storyIntroText, storyIntroFullText));
        pressSpaceToSkipIntroAnimator.SetBool("IsFading", true);

        yield return new WaitForSeconds(gameIntroDelay);

        introTextAnimator.SetBool("IsFading", true);
        introDescriptionAnimator.SetBool("IsFading", true);
        pressSpaceToSkipIntroAnimator.SetBool("IsFadingOut", true);

        yield return new WaitForSeconds(introTitleDescTextFadeOutDelay);

        EndGameIntro();
        fogForFirstTime = true;
        kidRoomLight.enabled = true;
        RenderSettings.fogDensity = 0.005f;
        StartCoroutine(AfterEndGameIntroDelay());
    }

    public void SettingsButton()
    {
        mainMenu.SetActive(false);

        SettingsUIManager.Instance.SettingsMenu.SetActive(true);
        SettingsUIManager.Instance.GetBackToMenu.SetActive(true);

        DisableRedColorTextFromMenuButtons();

        settingsButton.transform.DOScale(1f, 0.2f);
        RoundManager.Instance.CurrentMenuState = MenuState.OnMenuSettings;
    }

    public void CreditsButton()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        backToCreditsButton.SetActive(true);

        DisableRedColorTextFromMenuButtons();

        creditsButton.transform.DOScale(1f, 0.2f);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackToMenuCredits()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        backToCreditsButton.SetActive(false);

        backToCreditsMenuText.color = Color.white;
        backToMenuButtonCredits.transform.DOScale(3.2f, 0.2f);
        RoundManager.Instance.CurrentMenuState = MenuState.OnMainMenu;
    }

    public void DisableRedColorTextFromMenuButtons()
    {
        foreach (TextMeshProUGUI text in menuButtonsText)
        {
            text.color = Color.white;
        }
    }
}
