using UnityEngine;

public partial class Interactor
{
    public void HandleInputs()
    {
        if (IsInputBlocked()) return;

        if (Input.GetKeyDown(keybindManager.ActualKeybinds["Interact"]))
            TryInteract();

        if (Input.GetKeyDown(keybindManager.ActualKeybinds["Flashlight"]))
            ToggleFlashlight();
    }

    private void ToggleFlashlight()
    {
        if (!hasFlashlight || triggerFlickering.IsFlickering) return;

        if (gameManager.CurrentGameState == GameState.OnPlaying)
        {
            flashlight.Toggle();
            flashlight.flashlightAudioSourceObj.transform.position = equippedFlashlightObj.transform.position;
        }
        StartCoroutine(ToggleDelay());
    }

    private bool IsInputBlocked()
    {
        return !canToggle || gameManager.CurrentMenuState == MenuState.OnInventoryMenu
            || gameManager.CurrentMenuState == MenuState.OnNoteMenu;
    }
}
