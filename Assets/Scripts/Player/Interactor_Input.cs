using UnityEngine;

public partial class Interactor
{
    public void HandleInputs()
    {
        if (IsInputBlocked()) return;

        if (Input.GetKeyDown(KeybindManager.Instance.ActualKeybinds["Interact"]))
            TryInteract();

        if (Input.GetKeyDown(KeybindManager.Instance.ActualKeybinds["Flashlight"]))
            ToggleFlashlight();
    }

    private void ToggleFlashlight()
    {
        if (!hasFlashlight || triggerFlickering.IsFlickering) return;

        if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            flashlight.Toggle();
            flashlight.flashlightAudioSourceObj.transform.position = equippedFlashlightObj.transform.position;
        }
        StartCoroutine(ToggleDelay());
    }

    private bool IsInputBlocked()
    {
        return !canToggle || RoundManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu
            || RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu;
    }
}
