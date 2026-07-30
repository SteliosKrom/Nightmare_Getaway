using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    private float splashScreenDelay = 6f;
    private float titleMenuAnimationsDelay = 2f;

    public bool mainGameHasLoaded = false;

    #region SERVICES
    private GameManager gameManager;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject headsetPanel;
    [SerializeField] private GameObject seizurePanel;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    private Animator pressAnyKeyToStartAnimator;
    private Animator titleMenuAnimator;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(transform.root.gameObject);

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
        gameManager = ServiceManager.GetService<GameManager>();

        mainGameHasLoaded = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(ShowSplashScreens());
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGameScene")
        {
            InitializeAnimatorComponents();
            StartCoroutine(TitleMenuAnimationDelay());
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

        gameManager.CurrentMenuState = MenuState.OnTitleMenu;
    }

    public IEnumerator ShowSplashScreens()
    {
        LoadSeizureWarningPanel();
        yield return new WaitForSeconds(splashScreenDelay);
        LoadHeadsetPanel();
    }

    public void LoadHeadsetPanel()
    {
        headsetPanel.SetActive(true);
        seizurePanel.SetActive(false);
    }

    public void LoadSeizureWarningPanel()
    {
        headsetPanel.SetActive(false);
        seizurePanel.SetActive(true);
    }

    public void LoadGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainGameScene");
    }
}
