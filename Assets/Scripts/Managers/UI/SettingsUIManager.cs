using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    #region SERVICES
    private GameManager gameManager;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private MainMenuUIManager mainMenuUIManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private GameObject backToMenu;
    [SerializeField] private GameObject backToPauseMenu;
    [SerializeField] private GameObject backToSettings;
    [SerializeField] private GameObject backToSettingsFromGame;
    #endregion

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI backToMenuText;
    [SerializeField] private TextMeshProUGUI backToSettingsText;

    [SerializeField] private TextMeshProUGUI backToSettingsFromGameText;
    [SerializeField] private TextMeshProUGUI backToPauseMenuText;

    [SerializeField] private TextMeshProUGUI[] settingsCategoryTexts;

    [Header("BUTTONS")]
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button backToSettingsButton;

    [SerializeField] private Button backToPauseMenuButton;
    [SerializeField] private Button backToSettingsFromGameButton;

    [SerializeField] private Button[] settingsCategoryButtons;
    #endregion

    #region PROPERTIES
    public GameObject SettingsMenu => settingsMenu;
    public GameObject GetBackToMenu => backToMenu;
    public GameObject GetBackToSettings => backToSettings;
    public GameObject GetBackToPauseMenu => backToPauseMenu;
    public GameObject GetBackToSettingsFromGame => backToSettingsFromGame;

    public Button GetBackToSettingsButton => backToSettingsButton;
    public Button GetBackToSettingsFromGameButton => backToSettingsFromGameButton;
    #endregion

    private void Awake()
    {
        ServiceManager.RegisterService<SettingsUIManager>(this);
    }

    private void Start()
    {
        gameManager = ServiceManager.GetService<GameManager>();
    }

    public void OpenAudio()
    {
        ResetSettingsCategoryScale();
        DisableRedColorFromSettingsCategories();

        OpenCategory(audioMenu);
        gameManager.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenDisplay()
    {
        ResetSettingsCategoryScale();
        DisableRedColorFromSettingsCategories();

        OpenCategory(displayMenu);
        gameManager.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenGraphics()
    {
        ResetSettingsCategoryScale();
        DisableRedColorFromSettingsCategories();

        OpenCategory(graphicsMenu);
        gameManager.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenControls()
    {
        ResetSettingsCategoryScale();
        DisableRedColorFromSettingsCategories();

        OpenCategory(controlsMenu);
        gameManager.CurrentMenuState = MenuState.OnCategorySettings;
    }

    public void OpenCategory(GameObject category)
    {
        settingsMenu.SetActive(false);
        category.SetActive(true);

        switch (gameManager.CurrentMenuState)
        {
            case MenuState.OnMenuSettings:
                backToMenu.SetActive(false);
                backToSettings.SetActive(true);
                break;
            case MenuState.OnGameSettings:
                backToPauseMenu.SetActive(false);
                backToSettingsFromGame.SetActive(true);
                break;
        }
    }

    public void BackToMenu()
    {
        mainMenuUIManager.MainMenu.SetActive(true);
        backToMenu.SetActive(false);

        CloseAll();

        backToMenuText.color = Color.white;
        backToMenuButton.transform.DOScale(3.2f, 0.2f);

        gameManager.CurrentMenuState = MenuState.OnMainMenu;
    }

    public void BackToSettings()
    {
        settingsMenu.SetActive(true);
        backToSettings.SetActive(false);
        backToMenu.SetActive(true);

        HideAllCategories();

        backToSettingsText.color = Color.white;
        backToSettingsButton.transform.DOScale(3.2f, 0.2f);

        gameManager.CurrentMenuState = MenuState.OnMenuSettings;
    }

    public void BackToPauseMenu()
    {
        pauseMenu.SetActive(true);
        backToPauseMenu.SetActive(false);

        CloseAll();

        backToPauseMenuText.color = Color.white;
        backToPauseMenuButton.transform.DOScale(3.2f, 0.2f);

        gameManager.CurrentGameState = GameState.OnPause;
        gameManager.CurrentMenuState = MenuState.OnPauseMenu;
    }

    public void BackToSettingsFromGame()
    {
        settingsMenu.SetActive(true);
        backToPauseMenu.SetActive(true);
        backToSettingsFromGame.SetActive(false);

        HideAllCategories();

        backToSettingsFromGameText.color = Color.white;
        backToSettingsFromGame.transform.DOScale(3.2f, 0.2f);
        gameManager.CurrentMenuState = MenuState.OnGameSettings;
    }

    public void HideAllCategories()
    {
        audioMenu.SetActive(false);
        displayMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void CloseAll()
    {
        settingsMenu.SetActive(false);
        HideAllCategories();
    }

    public void DisableRedColorFromSettingsCategories()
    {
        foreach (TextMeshProUGUI text in settingsCategoryTexts)
        {
            text.color = Color.white;
        }
    }

    public void ResetSettingsCategoryScale()
    {
        foreach (Button button in settingsCategoryButtons)
        {
            button.transform.DOScale(4.0f, 0.2f);
        }
    }
}
