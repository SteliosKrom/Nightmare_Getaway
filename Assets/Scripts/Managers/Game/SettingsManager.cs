using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region CONSTANTS
    private const string masterVol = "MasterVolume";
    private const string gameVol = "GameVolume";
    private const string sfxVol = "SoundEffectsVolume";
    private const string menuVol = "MenuVolume";
    #endregion

    #region SERVICES
    private KeybindManager keybindManager;
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private CameraRotate cameraRotate;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject displayFPS;
    #endregion

    #region UI
    [Header("TOGGLE")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle framesToggle;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private Toggle motionBlurToggle;

    [Header("SLIDER")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider gameVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider menuVolumeSlider;

    [Header("DROPDOWN")]
    [SerializeField] private TMP_Dropdown antiAliasingDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private TextMeshProUGUI sfxValueText;
    [SerializeField] private TextMeshProUGUI menuValueText;
    [SerializeField] private TextMeshProUGUI gameValueText;
    [SerializeField] private TextMeshProUGUI antiAliasingText;
    [SerializeField] private TextMeshProUGUI framesText;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera secondaryCamera;
    #endregion

    #region POST-PROCESSING
    [Header("POST-PROCESSING")]
    [SerializeField] private Volume mainCameraVolume;
    [SerializeField] private Volume secondaryCameraVolume;

    private MotionBlur motionBlur;
    private ColorAdjustments mainCameraVolumeColorAdjustment;
    private ColorAdjustments secondaryCameraVolumeColorAdjustment;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField] private AudioMixer myAudioMixer;
    #endregion

    #region RENDERING
    [Header("RENDERING")]
    [SerializeField] private UniversalRenderPipelineAsset defaultUniversalPipelineAsset;
    #endregion

    private void Start()
    {
        keybindManager = ServiceManager.GetService<KeybindManager>();

        LoadSettings();
    }

    public void TestMotionBlurExistence()
    {
        if (mainCameraVolume.profile.TryGet(out motionBlur))
        {
            Debug.Log("Motion Blur override is present in the Volume Profile.");
        }
        else
        {
            Debug.Log("Motion Blur effect not found in the Volume Profile.");
        }
    }

    public void LoadSettings()
    {
        Resolution screenRes = Screen.currentResolution;

        TestMotionBlurExistence();

        // Load Audio values
        float savedSliderMasterVolume = PlayerPrefs.GetFloat("masterVolume", 0.75f);
        float savedActualMasterVolume = PlayerPrefs.GetFloat(masterVol, 2f);

        float savedSliderSfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        float savedActualSfxVolume = PlayerPrefs.GetFloat(sfxVol, 6f);

        float savedSliderMenuVolume = PlayerPrefs.GetFloat("menuVolume", 0.5f);
        float savedActualMenuVolume = PlayerPrefs.GetFloat(menuVol, -3f);

        float savedSliderGameVolume = PlayerPrefs.GetFloat("gameVolume", 0.65f);
        float savedActualGameVolume = PlayerPrefs.GetFloat(gameVol, 0.16f);

        masterVolumeSlider.value = savedSliderMasterVolume;
        sfxVolumeSlider.value = savedSliderSfxVolume;
        menuVolumeSlider.value = savedSliderMenuVolume;
        gameVolumeSlider.value = savedSliderGameVolume;

        myAudioMixer.SetFloat(masterVol, savedActualMasterVolume);
        myAudioMixer.SetFloat(sfxVol, savedActualSfxVolume);
        myAudioMixer.SetFloat(menuVol, savedActualMenuVolume);
        myAudioMixer.SetFloat(gameVol, savedActualGameVolume);

        // Load Video & Graphics values
        int savedQualitySettings = PlayerPrefs.GetInt("GraphicsQuality", 3);
        int savedVSyncSettings = PlayerPrefs.GetInt("VSyncCount", 1);
        int savedAntiAliasSettings = PlayerPrefs.GetInt("AntiAlias", 4);
        int savedAntiAliasValue = PlayerPrefs.GetInt("AntiAliasValue", 2);
        int savedResolutionWidth = PlayerPrefs.GetInt("ScreenWidth", screenRes.width);
        int savedResolutionHeight = PlayerPrefs.GetInt("ScreenHeight", screenRes.height);

        bool savedVSyncToggle = (PlayerPrefs.GetInt("VSyncToggleValue", 1) != 0);
        bool savedFullscreenValue = (PlayerPrefs.GetInt("ScreenValue", 1) != 0);
        bool savedFramesToggle = (PlayerPrefs.GetInt("Frames") != 0);
        bool savedMotionBlurToggle = (PlayerPrefs.GetInt("MotionBlurToggleValue", 0) != 0);
        bool savedMotionBlurEffect = (PlayerPrefs.GetInt("MotionBlurEffectValue", 0) != 0);

        float savedSensitivityValue = PlayerPrefs.GetFloat("SensValue", 1);

        float savedSliderGammaValue = PlayerPrefs.GetFloat("GammaSliderValue", 0.5f);
        float savedActualGammaValue = PlayerPrefs.GetFloat("ActualGammaExposureValue", 0.5f);

        // Load Keybinds Text
        LoadKeybindTexts();

        // Load actual keybinds
        LoadActualKeybinds();

        // Load Graphics & Display
        QualitySettings.vSyncCount = savedVSyncSettings;
        QualitySettings.SetQualityLevel(savedQualitySettings);
        Screen.SetResolution(savedResolutionWidth, savedResolutionHeight, savedFullscreenValue);
        defaultUniversalPipelineAsset.msaaSampleCount = savedAntiAliasSettings;

        if (mainCameraVolume.profile.TryGet(out mainCameraVolumeColorAdjustment))
            mainCameraVolumeColorAdjustment.postExposure.value = savedActualGammaValue;

        if (secondaryCameraVolume.profile.TryGet(out secondaryCameraVolumeColorAdjustment))
            secondaryCameraVolumeColorAdjustment.postExposure.value = savedActualGammaValue;

        // Dropdowns
        qualityDropdown.value = savedQualitySettings;
        antiAliasingDropdown.value = savedAntiAliasValue;

        // Sliders
        cameraRotate.SensitivitySlider = savedSensitivityValue;

        // Toggles
        fullscreenToggle.isOn = savedFullscreenValue;
        framesToggle.isOn = savedFramesToggle;
        vSyncToggle.isOn = savedVSyncToggle;
        motionBlurToggle.isOn = savedMotionBlurToggle;
        motionBlur.active = savedMotionBlurEffect;
    }

    public void LoadKeybindTexts()
    {
        foreach (var pair in keybindManager.KeybindsText)
        {
            string action = pair.Key;

            if (PlayerPrefs.HasKey(action + "_Text"))
            {
                pair.Value.text = PlayerPrefs.GetString(action + "_Text");
            }
            else
            {
                pair.Value.text = keybindManager.KeybindsText[action].text;
            }
        }
    }

    public void LoadActualKeybinds()
    {
        var keys = new List<string>(keybindManager.ActualKeybinds.Keys);

        foreach (var action in keys)
        {
            if (PlayerPrefs.HasKey(action + "_Key"))
            {
                string savedKey = PlayerPrefs.GetString(action + "_Key");
                keybindManager.ActualKeybinds[action] = (KeyCode)Enum.Parse(typeof(KeyCode), savedKey);
            }
        }
    }

    public void MasterVolumeSlider()
    {
        float masterVolume = masterVolumeSlider.value;
        float dB;
        float minMasterVolume = 0.0001f;
        masterValueText.text = masterVolume.ToString("0%");

        if (masterVolume <= minMasterVolume)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(masterVolume) * 20f;
            float boostAmount = 6f;
            dB += boostAmount * masterVolume;
        }

        myAudioMixer.SetFloat(masterVol, dB);
        PlayerPrefs.SetFloat(masterVol, dB);
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
    }

    public void SFXVolumeSlider()
    {
        float sfxVolume = sfxVolumeSlider.value;
        float dB;
        float minSFXVolume = 0.0001f;
        sfxValueText.text = sfxVolume.ToString("0%");

        if (sfxVolume <= minSFXVolume)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(sfxVolume) * 20f;
            float boostAmount = 6f;
            dB += boostAmount * sfxVolume;
        }

        myAudioMixer.SetFloat(sfxVol, dB);
        PlayerPrefs.SetFloat(sfxVol, dB);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public void GameVolumeSlider()
    {
        float gameVolume = gameVolumeSlider.value;
        float dB;
        float minGameVolume = 0.0001f;

        gameValueText.text = gameVolume.ToString("0%");

        if (gameVolume <= minGameVolume)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(gameVolume);
            float boostAmount = 6f;
            dB += boostAmount * gameVolume;
        }
        myAudioMixer.SetFloat(gameVol, dB);
        PlayerPrefs.SetFloat(gameVol, dB);
        PlayerPrefs.SetFloat("gameVolume", gameVolume);
    }

    public void MenuVolumeSlider()
    {
        float menuVolume = menuVolumeSlider.value;
        float dB;
        float minMenuVolume = 0.0001f;
        menuValueText.text = menuVolume.ToString("0%");

        if (menuVolume <= minMenuVolume)
        {
            dB = -80f;
        }
        else
        {
            dB = Mathf.Log10(menuVolume) * 20f;
            float boostAmount = 6f;
            dB += boostAmount * menuVolume;
        }

        myAudioMixer.SetFloat(menuVol, dB);
        PlayerPrefs.SetFloat(menuVol, dB);
        PlayerPrefs.SetFloat("menuVolume", menuVolume);
    }

    public void SetMotionBlur()
    {
        motionBlur.active = motionBlurToggle.isOn;
        PlayerPrefs.SetInt("MotionBlurEffectValue", (motionBlur.active ? 1 : 0));
        PlayerPrefs.SetInt("MotionBlurToggleValue", (motionBlurToggle.isOn ? 1 : 0));
        Debug.Log("Motion Blur: " + motionBlur.active);
        Debug.Log("Motion Blur Toggle: " + motionBlurToggle.isOn);
    }

    public void SetFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            Resolution nativeRes = Screen.currentResolution;
            int width = PlayerPrefs.GetInt("ScreenWidth", nativeRes.width);
            int height = PlayerPrefs.GetInt("ScreenHeight", nativeRes.height);
            Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            PlayerPrefs.SetInt("ScreenValue", 1);
        }
        else
        {
            int width = PlayerPrefs.GetInt("ScreenWidth", 1280);
            int height = PlayerPrefs.GetInt("ScreenHeight", 720);
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
            PlayerPrefs.SetInt("ScreenValue", 0);
        }
        PlayerPrefs.Save();
    }


    public void SetVSync()
    {
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSyncCount", QualitySettings.vSyncCount);
        PlayerPrefs.SetInt("VSyncToggleValue", (vSyncToggle.isOn ? 1 : 0));
    }

    public void SetAntiAliasing()
    {
        defaultUniversalPipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;

        switch (antiAliasingDropdown.value)
        {
            case 0:
                defaultUniversalPipelineAsset.msaaSampleCount = 1;
                break;
            case 1:
                defaultUniversalPipelineAsset.msaaSampleCount = 2;
                break;
            case 2:
                defaultUniversalPipelineAsset.msaaSampleCount = 4;
                break;
            case 3:
                defaultUniversalPipelineAsset.msaaSampleCount = 8;
                break;
        }
        PlayerPrefs.SetInt("AntiAlias", defaultUniversalPipelineAsset.msaaSampleCount);
        PlayerPrefs.SetInt("AntiAliasValue", antiAliasingDropdown.value);
    }

    public void SetGraphicsQuality()
    {
        int qualityValue = qualityDropdown.value;
        QualitySettings.SetQualityLevel(qualityValue);

        switch (qualityValue)
        {
            case 0:
                defaultUniversalPipelineAsset.msaaSampleCount = 1;
                antiAliasingDropdown.value = 0;
                break;
            case 1:
                defaultUniversalPipelineAsset.msaaSampleCount = 2;
                antiAliasingDropdown.value = 1;
                break;
            case 2:
                defaultUniversalPipelineAsset.msaaSampleCount = 4;
                antiAliasingDropdown.value = 2;
                break;
            case 3:
                defaultUniversalPipelineAsset.msaaSampleCount = 4;
                antiAliasingDropdown.value = 2;
                break;
            case 4:
                defaultUniversalPipelineAsset.msaaSampleCount = 8;
                antiAliasingDropdown.value = 3;
                break;
            case 5:
                defaultUniversalPipelineAsset.msaaSampleCount = 8;
                antiAliasingDropdown.value = 3;
                break;
        }
        PlayerPrefs.SetInt("AntiAlias", defaultUniversalPipelineAsset.msaaSampleCount);
        PlayerPrefs.SetInt("AntiAliasValue", antiAliasingDropdown.value);
        PlayerPrefs.SetInt("GraphicsQuality", qualityValue);
    }

    public void SetFPS()
    {
        displayFPS.SetActive(framesToggle.isOn);
        PlayerPrefs.SetInt("Frames", (framesToggle.isOn ? 1 : 0));
    }
}
