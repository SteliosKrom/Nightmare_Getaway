using NUnit.Framework.Internal.Filters;
using System.Collections.Generic;
using UnityEngine;

public partial class Interactor
{
    public void TryInteract()
    {
        if (!Physics.Raycast(interactionSource.position, interactionSource.forward, out RaycastHit hit, interactionRange, interactableLayer | obstacleLayer))
            return;

        if (hit.collider.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact(this);
        }
    }

    public void DetectInteractable()
    {
        if (RoundManager.Instance.CurrentGameState != GameState.OnPlaying)
            return;
        if (RoundManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu ||
            RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu) return;

        LayerMask combinedMask = interactableLayer | obstacleLayer;

        if (!Physics.Raycast(
            interactionSource.position,
            interactionSource.forward,
            out RaycastHit hit,
            interactionRange,
            combinedMask))
        {
            HUD.Instance.ShowDotOnly();
            return;
        }

        if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
        {
            HUD.Instance.ShowDotOnly();
            return;
        }

        if (hit.collider.TryGetComponent(out DoorBase door))
        {
            bool isLocked = door.tag switch
            {
                "KidsDoor" => RoundManager.Instance.CurrentKidsDoorState != KidsDoorState.unlocked,
                "GarageDoor" => RoundManager.Instance.CurrentGarageDoorState != GarageDoorState.unlocked,
                "MainDoor" => RoundManager.Instance.CurrentMainDoorState != MainDoorState.unlocked,
                _ => false
            };

            if (isLocked)
                HUD.Instance.ShowLockedOnly();
            else
                HUD.Instance.ShowInteract();

            return;
        }

        if (hit.collider.TryGetComponent<IInteractable>(out _))
        {
            HUD.Instance.ShowInteract();
            return;
        }

        HUD.Instance.ShowDotOnly();
    }


    void DebugRaycast()
    {
        if (interactionSource != null)
            Debug.DrawRay(interactionSource.position, interactionSource.forward * interactionRange, Color.red);
    }
}
