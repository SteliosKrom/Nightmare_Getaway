using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public static Transition Instance;

    [Header("GENERAL")]
    private float splashScreenDelay = 6f;
    private float titleMenuAnimationsDelay = 2f;

    private bool isOnBrightnessPanel = true;
    public bool mainGameHasLoaded = false;

    [Header("GAME OBJECTS")]
    [SerializeField] private GameObject headsetPanel;
    [SerializeField] private GameObject seizurePanel;
    [SerializeField] private GameObject brightnessCalibrationPanelLogo;
    [SerializeField] private GameObject brightnessCalibrationPanelUI;

    [Header("ANIMATIONS")]
     private Animator pressAnyKeyToStartAnimator;
     private Animator titleMenuAnimator;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }

        if (!PlayerPrefs.HasKey("GraphicsQuality"))
        {
            PlayerPrefs.SetInt("GraphicsQuality", 3);
            PlayerPrefs.Save();
        }
        int savedGraphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 3);
        QualitySettings.SetQualityLevel(savedGraphicsQuality);
    }

    private void Start()
    {
        mainGameHasLoaded = false;
        isOnBrightnessPanel = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(ShowSplashScreens());
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isOnBrightnessPanel)
        {
            isOnBrightnessPanel = false;
            LoadGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGameScene")
        {
            InitializeAnimatorComponents();
            StartCoroutine(TitleMenuAnimationDelay());
        }
        else
        {
            return;
        }
    }

    public void InitializeAnimatorComponents()
    {
        titleMenuAnimator = GameObject.Find("TitleMenu").GetComponent<Animator>();
        pressAnyKeyToStartAnimator = GameObject.Find("PressAnyKeyToStart").GetComponent<Animator>();
    }

    public IEnumerator TitleMenuAnimationDelay()
    {
        titleMenuAnimator.SetBool("IsFadingIn", false);
        pressAnyKeyToStartAnimator.SetBool("IsFadingIn", false);
        yield return new WaitForSeconds(titleMenuAnimationsDelay);
        titleMenuAnimator.SetBool("IsFadingIn", true);
        pressAnyKeyToStartAnimator.SetBool("IsFadingIn", true);
        yield return new WaitForSeconds(titleMenuAnimationsDelay);
        titleMenuAnimator.SetBool("IsOn", true);
        pressAnyKeyToStartAnimator.SetBool("IsFading", true);
        RoundManager.Instance.CurrentMenuState = MenuState.OnTitleMenu;
    }

    public IEnumerator ShowSplashScreens()
    {
        LoadSeizureWarningPanel();
        yield return new WaitForSeconds(splashScreenDelay);
        LoadHeadsetPanel();
        yield return new WaitForSeconds(splashScreenDelay);
        LoadBrightnessCalibrationPanel();
    }

    public void LoadHeadsetPanel()
    {
        headsetPanel.SetActive(true);
        seizurePanel.SetActive(false);
        brightnessCalibrationPanelLogo.SetActive(false);
        brightnessCalibrationPanelUI.SetActive(false);
    }

    public void LoadSeizureWarningPanel()
    {
        headsetPanel.SetActive(false);
        seizurePanel.SetActive(true);
        brightnessCalibrationPanelLogo.SetActive(false);
        brightnessCalibrationPanelUI.SetActive(false);
    }

    public void LoadBrightnessCalibrationPanel()
    {
        isOnBrightnessPanel = true;
        headsetPanel.SetActive(false);
        seizurePanel.SetActive(false);
        brightnessCalibrationPanelLogo.SetActive(true);
        brightnessCalibrationPanelUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        BrightnessManager.Instance.LoadLogoGammaCorrection();
    }

    public void LoadGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainGameScene");
    }
}
