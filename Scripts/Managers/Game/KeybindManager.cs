using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    private string currentActionName;

    [SerializeField] private bool isWaitingForKey = false;
    private bool coroutineInProgress = false;

    private float keybindsDelay = 1f;
    private float isWaitingForKeyDelay = 0.1f;

    #region UI
    [Header("TEXT")]
    [SerializeField] private TextMeshProUGUI moveForwardText;
    [SerializeField] private TextMeshProUGUI moveBackwardText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI crouchText;
    [SerializeField] private TextMeshProUGUI sprintText;
    [SerializeField] private TextMeshProUGUI flashlightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private TextMeshProUGUI pauseText;
    #endregion

    #region OBJECTS
    [Header("OBJECTS")]
    [SerializeField] private GameObject enterAKeyText;
    [SerializeField] private GameObject keyAssignedText;
    [SerializeField] private GameObject keybindsPanel;
    [SerializeField] private GameObject backToPreviousGameSettings;
    [SerializeField] private GameObject backToPreviousMenuSettings;
    #endregion

    #region KEYBINDS
    private Dictionary<string, KeyCode> actualKeybinds;
    private Dictionary<string, TextMeshProUGUI> keybindsText;
    #endregion

    public bool IsWaitingForKey { get { return isWaitingForKey; } set { isWaitingForKey = value; } }
    public Dictionary<string, KeyCode> ActualKeybinds { get { return actualKeybinds; } set { actualKeybinds = value; } }
    public Dictionary<string, TextMeshProUGUI> KeybindsText { get { return keybindsText; } set { keybindsText = value; } }

    private void Awake()
    {
        actualKeybinds = new Dictionary<string, KeyCode>
        {
            {"MoveForward", KeyCode.W},
            {"MoveBackward", KeyCode.S},
            {"MoveLeft", KeyCode.A},
            {"MoveRight", KeyCode.D},
            {"Crouch", KeyCode.C},
            {"Sprint", KeyCode.LeftShift},
            {"Flashlight", KeyCode.F},
            {"Interact", KeyCode.E},
            {"Inventory", KeyCode.I},
            {"Pause", KeyCode.Escape}
        };

        keybindsText = new Dictionary<string, TextMeshProUGUI>
        {
            {"MoveForward", moveForwardText},
            {"MoveBackward", moveBackwardText},
            {"MoveLeft", moveLeftText},
            {"MoveRight", moveRightText},
            {"Crouch", crouchText},
            {"Sprint", sprintText},
            {"Flashlight", flashlightText},
            {"Interact", interactText},
            {"Inventory", inventoryText},
            {"Pause", pauseText}
        };

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isWaitingForKey == true)
        {
            HandleKeybinds();
        }
    }

    public void HandleKeybinds()
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                if (IsKeyUsed(keycode))
                {
                    if (coroutineInProgress == false)
                        StartCoroutine(KeybindsDelay(currentActionName));

                    return;
                }
                else
                {
                    keybindsPanel.SetActive(false);
                    enterAKeyText.SetActive(false);

                    if (RoundManager.Instance.CurrentMenuState == MenuState.OnMenuSettings)
                    {
                        backToPreviousMenuSettings.SetActive(true);
                    }
                    else if (RoundManager.Instance.CurrentMenuState == MenuState.OnGameSettings)
                    {
                        backToPreviousGameSettings.SetActive(true);
                    }

                    keybindsText[currentActionName].text = GetReadableKeyName(keycode);
                    actualKeybinds[currentActionName] = keycode;
                    keybindsText[currentActionName].enabled = true;

                    PlayerPrefs.SetString(currentActionName + "_Text", GetReadableKeyName(keycode));
                    PlayerPrefs.SetString(currentActionName + "_Key", keycode.ToString());

                    StartCoroutine(IsWaitingForKeyDelay());
                    break;
                }
            }
        }
    }

    public IEnumerator IsWaitingForKeyDelay()
    {
        yield return new WaitForSecondsRealtime(isWaitingForKeyDelay);
        isWaitingForKey = false;
    }

    public void ChangeKey(TextMeshProUGUI keybindText)
    {
        foreach (var pair in keybindsText)
        {
            if (pair.Value == keybindText)
            {
                currentActionName = pair.Key;
                pair.Value.enabled = false;
                break;
            }
        }

        if (RoundManager.Instance.CurrentMenuState == MenuState.OnMenuSettings)
            backToPreviousMenuSettings.SetActive(false);
        else if (RoundManager.Instance.CurrentMenuState == MenuState.OnGameSettings)
            backToPreviousGameSettings.SetActive(false);

        keybindsPanel.SetActive(true);
        enterAKeyText.SetActive(true);
        keyAssignedText.SetActive(false);
        isWaitingForKey = true;
    }

    public bool IsKeyUsed(KeyCode key)
    {
        foreach (var action in actualKeybinds)
        {
            if (action.Value == key && action.Key != currentActionName)
                return true;
        }
        return false;
    }

    public IEnumerator KeybindsDelay(string actionName)
    {
        coroutineInProgress = true;

        keyAssignedText.SetActive(true);
        enterAKeyText.SetActive(false);

        yield return new WaitForSecondsRealtime(keybindsDelay);

        isWaitingForKey = true;
        KeyCode currentKey = actualKeybinds[actionName];
        keybindsText[actionName].text = GetReadableKeyName(currentKey);

        coroutineInProgress = false;
    }

    public string GetReadableKeyName(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0: return "LMB";
            case KeyCode.Mouse1: return "RMB";
            case KeyCode.Mouse2: return "MMB";

            case KeyCode.Mouse3: return "MB4";
            case KeyCode.Mouse4: return "MB5";
            case KeyCode.Mouse5: return "MB6";
            case KeyCode.Mouse6: return "MB7";

            case KeyCode.LeftShift: return "L-Shift";
            case KeyCode.RightShift: return "R-Shift";

            case KeyCode.LeftControl: return "L-Ctrl";
            case KeyCode.RightControl: return "R-Ctrl";

            case KeyCode.LeftAlt: return "L-Alt";
            case KeyCode.RightAlt: return "R-Alt";

            case KeyCode.Return: return "Enter";
            case KeyCode.Escape: return "Esc";
            case KeyCode.Space: return "Space";

            case KeyCode.BackQuote: return "`";
            case KeyCode.Period: return ".";
            case KeyCode.Comma: return ",";
            case KeyCode.Slash: return "/";
            case KeyCode.Backslash: return "\\";
            case KeyCode.Equals: return "=";
            case KeyCode.Minus: return "-";
            case KeyCode.Semicolon: return ";";
            case KeyCode.Quote: return "'";

            default: return key.ToString();
        }


    }
}
