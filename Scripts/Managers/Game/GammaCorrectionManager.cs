using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using System.Collections;

public class BrightnessManager : MonoBehaviour
{
    public static BrightnessManager Instance;

    #region GENERAL
    private float savedSliderValue;
    private float exposure;
    #endregion

    #region UI
    [Header("UI")]
    private TextMeshProUGUI gammaValueText;
    private Slider gammaCorrectionSlider;
    #endregion

    #region POST-PROCESSING
    [Header("POST-PROCESSING")]
    private Volume logoCameraVolume;
    private Volume mainCameraVolume;
    private Volume secondaryCameraVolume;

    private ColorAdjustments logoCameraColorAdjustment;
    private ColorAdjustments mainCameraColorAdjustment;
    private ColorAdjustments secondaryCameraColorAdjustment;
    #endregion

    public float SavedSliderValue { get => savedSliderValue; set => savedSliderValue = value; }
    public float Exposure { get => exposure; set => exposure = value; }

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
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "SplashScreen")
        {
            InitializeComponentsOfSplashScreenScene();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGameScene")
        {
            InitializeComponentsOfMainGameScene();
        }
        
        if (scene.name == "SplashScreen")
        {
            InitializeComponentsOfSplashScreenScene();
        }
    }

    public void InitializeComponentsOfMainGameScene()
    {
        mainCameraVolume = GameObject.Find("MainCamera").GetComponent<Volume>();
        secondaryCameraVolume = GameObject.Find("SecondaryCamera").GetComponent<Volume>();
    }

    public void InitializeComponentsOfSplashScreenScene()
    {
        GameObject UICanvas = GameObject.Find("UICanvas");
        Transform gammaUIPanel = UICanvas.transform.Find("GammaCalibrationPanelUI");

        gammaCorrectionSlider = gammaUIPanel.Find("GammaSlider").GetComponent<Slider>();
        gammaValueText = gammaUIPanel.Find("GammaValueText").GetComponent<TextMeshProUGUI>();
        logoCameraVolume = GameObject.Find("LogoCamera").GetComponent<Volume>();
    }

    public void LoadLogoGammaCorrection()
    {
        if (logoCameraVolume.profile.TryGet(out logoCameraColorAdjustment))
            logoCameraColorAdjustment.postExposure.value = -2f;
    }

    public void SetGammaCorrection()
    {
        float sliderValue = gammaCorrectionSlider.value;
        float exposure = Mathf.Lerp(-4.5f, 0f, sliderValue);

        gammaValueText.text = Mathf.RoundToInt(sliderValue * 100f) + "%";

        if (logoCameraVolume.profile.TryGet(out logoCameraColorAdjustment))
            logoCameraColorAdjustment.postExposure.value = exposure;

        if (mainCameraVolume != null && mainCameraVolume.profile.TryGet(out mainCameraColorAdjustment))
            mainCameraColorAdjustment.postExposure.value = exposure;

        if (secondaryCameraVolume != null && secondaryCameraVolume.profile.TryGet(out secondaryCameraColorAdjustment))
            secondaryCameraColorAdjustment.postExposure.value = exposure;

        PlayerPrefs.SetFloat("GammaSliderValue", sliderValue);
        PlayerPrefs.SetFloat("ActualGammaExposureValue", exposure);
    }
}

