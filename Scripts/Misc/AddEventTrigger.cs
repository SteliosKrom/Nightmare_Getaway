using DG.Tweening;
using JetBrains.Annotations;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddEventTrigger : MonoBehaviour
{
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI[] menuButtonTexts;
    [SerializeField] private TextMeshProUGUI[] settingsButtonTexts;
    [SerializeField] private TextMeshProUGUI[] gameButtonTexts;

    [Header("SETTINGS BUTTONS")]
    [SerializeField] private Button audioCategoryButton;
    [SerializeField] private Button videoCategoryButton;
    [SerializeField] private Button graphicsCategoryButton;
    [SerializeField] private Button controlsCategoryButon;
    [SerializeField] private Button backToPreviousFromMenuButton;

    [Header("MAIN MENU BUTTONS")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button backToMenuButtonCredits;
    [SerializeField] private Button backToMenuButtonSettings;

    [Header("MAIN GAME BUTTONS")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseSettingsButton;
    [SerializeField] private Button pauseHomeButton;
    [SerializeField] private Button pauseExitButton;
    [SerializeField] private Button backToGameButton;
    [SerializeField] private Button backToPreviousFromGameButton;

    [Header("INVENTORY BUTTONS")]
    [SerializeField] private Button[] itemButtons;

    private void Start()
    {
        DOTween.Init();
        DOTween.defaultTimeScaleIndependent = true;

        AttachButtonHoverEventsMenu(playButton, menuButtonTexts[0]);
        AttachButtonHoverEventsMenu(settingsButton, menuButtonTexts[1]);
        AttachButtonHoverEventsMenu(creditsButton, menuButtonTexts[2]);
        AttachButtonHoverEventsMenu(exitButton, menuButtonTexts[3]);

        AttachButtonHoverEventsSettings(audioCategoryButton, settingsButtonTexts[0]);
        AttachButtonHoverEventsSettings(videoCategoryButton, settingsButtonTexts[1]);
        AttachButtonHoverEventsSettings(graphicsCategoryButton, settingsButtonTexts[2]);
        AttachButtonHoverEventsSettings(controlsCategoryButon, settingsButtonTexts[3]);

        AttachButtonHoverEventsBack(backToMenuButtonSettings, settingsButtonTexts[4]);
        AttachButtonHoverEventsBack(backToGameButton, settingsButtonTexts[5]);
        AttachButtonHoverEventsBack(backToPreviousFromMenuButton, settingsButtonTexts[6]);
        AttachButtonHoverEventsBack(backToMenuButtonCredits, settingsButtonTexts[7]);
        AttachButtonHoverEventsBack(backToPreviousFromGameButton, settingsButtonTexts[8]);

        AttachButtonHoverEventsPause(resumeButton, gameButtonTexts[0]);
        AttachButtonHoverEventsPause(pauseSettingsButton, gameButtonTexts[1]);
        AttachButtonHoverEventsPause(pauseHomeButton, gameButtonTexts[2]);
        AttachButtonHoverEventsPause(pauseExitButton, gameButtonTexts[3]);
    }

    public void EnterHoverEffectPause(Transform buttonTransform)
    {
        buttonTransform.DOScale(1f, 0.2f).SetUpdate(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Hover.source, AudioManager.Instance.Hover.clip); 
    }

    public void ExitHoverEffectPause(Transform buttonTransform)
    {
        buttonTransform.DOScale(0.8f, 0.2f).SetUpdate(true);
    }

    public void EnterHoverEffectBack(Transform buttonTransform)
    {
        buttonTransform.DOScale(3.5f, 0.2f).SetUpdate(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Hover.source, AudioManager.Instance.Hover.clip);
    }

    public void ExitHoverEffectBack(Transform buttonTransform)
    {
        buttonTransform.DOScale(3.2f, 0.2f).SetUpdate(true);
    }

    public void EnterHoverEffectOther(Transform buttonTransform)
    {
        buttonTransform.DOScale(3.5f, 0.2f).SetUpdate(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Hover.source, AudioManager.Instance.Hover.clip);
    }

    public void ExitHoverEffectOther(Transform buttonTransform)
    {
        buttonTransform.DOScale(3.2f, 0.2f).SetUpdate(true);
    }

    public void EnterHoverEffectMenu(Transform buttonTransform)
    {
        buttonTransform.DOScale(1.2f, 0.2f).SetUpdate(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Hover.source, AudioManager.Instance.Hover.clip);
    }

    public void ExitHoverSoundEffectMenu(Transform buttonTransform)
    {
        buttonTransform.DOScale(1f, 0.2f).SetUpdate(true);
    }

    public void EnterHoverSoundEffectSettings(Transform buttonTransform)
    {
        buttonTransform.DOScale(4.5f, 0.2f).SetUpdate(true);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.Hover.source, AudioManager.Instance.Hover.clip);
    }

    public void ExitHoverSoundEffectSettings(Transform buttonTransform)
    {
        buttonTransform.DOScale(4f, 0.2f).SetUpdate(true);
    }

    public void AttachButtonHoverEventsMenu(Button menuButtons, TextMeshProUGUI menuButtonText)
    {
        EventTrigger menuTrigger = menuButtons.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry menuEntryEnter = new EventTrigger.Entry();
        menuEntryEnter.eventID = EventTriggerType.PointerEnter;
        menuEntryEnter.callback.AddListener((data) 
            => { EnterHoverEffectMenu(menuButtons.transform); menuButtonText.color = Color.red; });
        menuTrigger.triggers.Add(menuEntryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) 
            => { ExitHoverSoundEffectMenu(menuButtons.transform); menuButtonText.color = Color.white; });
        menuTrigger.triggers.Add(entryExit);
    }

    public void AttachButtonHoverEventsSettings(Button settingsButtons, TextMeshProUGUI settingsButtonText)
    {
        EventTrigger settingsTrigger = settingsButtons.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry settingsEntryEnter = new EventTrigger.Entry();
        settingsEntryEnter.eventID = EventTriggerType.PointerEnter;
        settingsEntryEnter.callback.AddListener((data) 
            => { EnterHoverSoundEffectSettings(settingsButtons.transform); settingsButtonText.color = Color.red; });
        settingsTrigger.triggers.Add(settingsEntryEnter);

        EventTrigger.Entry settingsEntryExit = new EventTrigger.Entry();
        settingsEntryExit.eventID = EventTriggerType.PointerExit;
        settingsEntryExit.callback.AddListener((data) 
            => { ExitHoverSoundEffectSettings(settingsButtons.transform); settingsButtonText.color = Color.white; });
        settingsTrigger.triggers.Add(settingsEntryExit);
    }

    public void AttachButtonHoverEventsBack(Button otherButtons, TextMeshProUGUI backButtonText)
    {
        EventTrigger otherTrigger = otherButtons.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry otherEntryEnter = new EventTrigger.Entry();
        otherEntryEnter.eventID = EventTriggerType.PointerEnter;
        otherEntryEnter.callback.AddListener((data)
            => { EnterHoverEffectBack(otherButtons.transform); backButtonText.color = Color.red; });
        otherTrigger.triggers.Add(otherEntryEnter);

        EventTrigger.Entry otherEntryExit = new EventTrigger.Entry();
        otherEntryExit.eventID = EventTriggerType.PointerExit;
        otherEntryExit.callback.AddListener((data)
            => { ExitHoverEffectBack(otherButtons.transform); backButtonText.color = Color.white; });
        otherTrigger.triggers.Add(otherEntryExit);
    }

    public void AttachButtonHoverEventsOther(Button otherButtons)
    {
        EventTrigger otherTrigger = otherButtons.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry otherEntryEnter = new EventTrigger.Entry();
        otherEntryEnter.eventID = EventTriggerType.PointerEnter;
        otherEntryEnter.callback.AddListener((data) 
            => { EnterHoverEffectOther(otherButtons.transform);});
        otherTrigger.triggers.Add(otherEntryEnter);

        EventTrigger.Entry otherEntryExit = new EventTrigger.Entry();
        otherEntryExit.eventID = EventTriggerType.PointerExit;
        otherEntryExit.callback.AddListener((data) 
            => ExitHoverEffectOther(otherButtons.transform));
        otherTrigger.triggers.Add(otherEntryExit);
    }

    public void AttachButtonHoverEventsPause(Button pauseButtons, TextMeshProUGUI pauseButtonText)
    {
        EventTrigger pauseTrigger = pauseButtons.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pauseEntryEnter = new EventTrigger.Entry();
        pauseEntryEnter.eventID = EventTriggerType.PointerEnter;
        pauseEntryEnter.callback.AddListener((data) 
            => { EnterHoverEffectPause(pauseButtons.transform); pauseButtonText.color = Color.red; });
        pauseTrigger.triggers.Add(pauseEntryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) 
            => { ExitHoverEffectPause(pauseButtons.transform); pauseButtonText.color = Color.white; });
        pauseTrigger.triggers.Add(entryExit);
    }
}
