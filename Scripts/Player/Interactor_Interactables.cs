using System.Collections.Generic;
using UnityEngine;

public partial class Interactor
{
    // --- Unified Door Mapping ---
    private readonly Dictionary<string, int> doorColliderMapping = new()
    {
        {"SecondBedroomDoor", 4},
        {"SecondBathroomDoor", 3},
        {"ThirdBathroomDoor", 7},
        {"ClothingsDoor", 1},
        {"BedroomDoor", 2},
        {"BathroomDoor", 6},
        {"KidsDoor", 0},
        {"GarageDoor", 5},
        {"MainDoor", 15}
    };

    // --- Main entry for handling any interactable object ---
    public void HandleInteractableGameObject(IInteractable interactableObj)
    {
        switch (interactableObj)
        {
            case Interactable interactable:
                HandleCollectableItem(interactable);
                break;
            case DoorBase door:
                HandleDoor(door);
                break;
            case OtherInteractable other:
                HandleSwitch(other.gameObject);
                break;
            case OtherInteractableSwitches switches:
                HandleSwitch(switches.gameObject);
                break;
            default:
                Debug.LogWarning("Unknown interactable type: " + interactableObj);
                break;
        }
    }

    private void HandleSwitch(GameObject switchObj)
    {
        string tag = switchObj.tag;

        if (tag == "KidRoomSwitch" || tag == "Switches")
        {
            kidRoomLight.enabled = !kidRoomLight.enabled;
            kidsRoomVolume.SetActive(kidRoomLight.enabled);
            PlayLightSFX();
        }
        else if (tag == "GarageSwitch")
        {
            isToggled = !isToggled;
            garageLightAnimator.SetBool("IsOn", isToggled);

            if (!isToggled)
                StartCoroutine(GarageLightBreakDelay());

            PlayLightSFX();
        }
        StartCoroutine(ToggleDelay());
    }


    // --- Handle Doors ---
    private void HandleDoor(DoorBase doorBase)
    {
        string tag = doorBase.gameObject.tag;

        if (!doorColliderMapping.TryGetValue(tag, out int colliderIndex))
        {
            Debug.LogWarning($"Unknown door tag: {tag}");
            return;
        }

        // Check if door is locked based on your current game state
        bool isLocked = tag switch
        {
            "KidsDoor" => RoundManager.Instance.CurrentKidsDoorState != KidsDoorState.unlocked,
            "GarageDoor" => RoundManager.Instance.CurrentGarageDoorState != GarageDoorState.unlocked,
            "MainDoor" => RoundManager.Instance.CurrentMainDoorState != MainDoorState.unlocked,
            _ => false
        };

        if (isLocked)
        {
            if (lockedCoroutineIsRunning) return;

            HUD.Instance.LockedIcon.SetActive(true);
            HUD.Instance.InteractIcon.SetActive(false);
            HUD.Instance.DotIcon.SetActive(false);
            StartCoroutine(LockedUIDelay());
            return;
        }
        else
        {
            HUD.Instance.LockedIcon.SetActive(false);
            HUD.Instance.InteractIcon.SetActive(true);
            HUD.Instance.DotIcon.SetActive(false);
        }

        // Interact with the door and handle colliders
        InteractWithDoor(doorBase, colliderIndex);

        // Move door audio sources to proper position
        if (interactableObjects.Count > colliderIndex)
        {
            closedDoorAudioSourceObject.transform.position = interactableObjects[colliderIndex].transform.position;
            openedDoorAudioSourceObject.transform.position = interactableObjects[colliderIndex].transform.position;
        }
    }

    // --- Handle Door Interaction with Colliders ---
    private void InteractWithDoor(DoorBase doorBase, int colliderIndex)
    {
        doorBase.OnDoorInteract();
        StartCoroutine(DoorCollidersDelay(doorColliders[colliderIndex]));
        StartCoroutine(doorHandleCollidersDelay(doorHandleColliders[colliderIndex]));
    }

    // --- Handle Collectable Items ---
    private void HandleCollectableItem(Interactable interactable)
    {
        bool hasCrucifix = RoundManager.Instance.CurrentItemState == ItemState.cross;
        bool hasKnife = RoundManager.Instance.CurrentItemState == ItemState.knife;
        bool hasBook = RoundManager.Instance.CurrentItemState == ItemState.book;

        GameObject obj = interactable.gameObject;
        string tag = obj.tag;

        void PlayEquipSFX()
        {
            AudioManager.Instance.EquipItem.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.EquipItem.source, AudioManager.Instance.EquipItem.clip);
        }

        switch (tag)
        {
            case "RoomKey":
                RoundManager.Instance.CurrentItemState = ItemState.kidsRoomKey;
                RoundManager.Instance.CurrentKidsDoorState = KidsDoorState.unlocked;
                taskManager.CompleteTask();
                doll.SetActive(true);
                roomKey.SetActive(false);
                radioAudioSource.Play();
                radioAudioLowPassFilter.cutoffFrequency = 3000f;
                inventory.AddToInventory(4);
                keyCounter++;
                keysCounter.text = $"x{keyCounter}";
                PlayEquipSFX();
                break;
            case "Radio":
                taskManager.CompleteTask();
                radioAudioSource.Stop();
                doorBoxCollider.enabled = false;
                garageKey.SetActive(true);
                radioCollider.enabled = false;
                break;
            case "Telephone":
                taskManager.CompleteTask();
                telephoneCollider.enabled = false;
                telephoneAudioSource.Stop();
                cursedBook.SetActive(true);
                break;
            case "Phone":
                taskManager.CompleteTask();
                phone.SetActive(false);
                creepyEntityTrigger.SetActive(true);
                creepyEntity.SetActive(true);
                candleLight.SetActive(true);
                creepyEntityTriggerObstacle.enabled = true;
                inventory.AddToInventory(5);
                PlayEquipSFX();
                StartCoroutine(HeartbeatAudioDelay());
                StartCoroutine(PhoneCallDelay());
                break;

            case "Flashlight":
                hasFlashlight = true;
                flashlightObj.SetActive(false);
                inventory.AddToInventory(3);
                PlayEquipSFX();
                break;
            case "GarageKey":
                RoundManager.Instance.CurrentItemState = ItemState.garageKey;
                RoundManager.Instance.CurrentGarageDoorState = GarageDoorState.unlocked;
                phone.SetActive(true);
                garageKey.SetActive(false);
                demonCryCollider.enabled = true;
                doorKnockTrigger.SetActive(true);
                keyCounter++;
                keysCounter.text = $"x{keyCounter}";
                PlayEquipSFX();
                break;
            case "MainDoorKey":
                RoundManager.Instance.CurrentItemState = ItemState.mainDoorKey;
                RoundManager.Instance.CurrentMainDoorState = MainDoorState.unlocked;
                taskManager.CompleteTask();
                mainDoorKey.SetActive(false);
                keyCounter++;
                keysCounter.text = $"x{keyCounter}";
                PlayEquipSFX();
                break;
            case "Book":
                RoundManager.Instance.CurrentItemState = ItemState.book;
                cursedBook.SetActive(false);
                inventory.AddToInventory(0);
                PlayEquipSFX();
                break;
            case "Cross":
                RoundManager.Instance.CurrentItemState = ItemState.cross;
                cursedCrucifix.SetActive(false);
                inventory.AddToInventory(2);
                PlayEquipSFX();
                break;
            case "Knife":
                RoundManager.Instance.CurrentItemState = ItemState.knife;
                cursedKnife.SetActive(false);
                inventory.AddToInventory(1);
                PlayEquipSFX();
                break;
            case "Note":
                RoundManager.Instance.CurrentMenuState = MenuState.OnNoteMenu;
                note.SetActive(false);
                noteMenu.SetActive(true);
                inventory.AddToInventory(6);
                AudioManager.Instance.CollectNote.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.CollectNote.source, AudioManager.Instance.CollectNote.clip);
                break;
            case "Table":
                if (hasBook)
                {
                    tableBook.SetActive(true);
                    cursedCrucifix.SetActive(true);
                }
                else if (hasCrucifix)
                {
                    tableCrucifix.SetActive(true);
                    cursedKnife.SetActive(true);
                }
                else if (hasKnife)
                {
                    taskManager.CompleteTask();
                    tableKnife.SetActive(true);
                    mainDoorKey.SetActive(true);
                    AudioManager.Instance.PlaySFX(demonCryAudioSource, demonCryAudioClip);
                    candleLight.SetActive(false);
                    candleSmoke.Play();
                }
                AudioManager.Instance.PlaceItem.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
                AudioManager.Instance.PlaySFX(AudioManager.Instance.PlaceItem.source, AudioManager.Instance.PlaceItem.clip);
                RoundManager.Instance.CurrentItemState = ItemState.none;
                break;
        }
    }

    // --- Utility ---
    private void PlayLightSFX()
    {
        AudioManager.Instance.LightSwitches.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LightSwitches.source, AudioManager.Instance.LightSwitches.clip);
    }
}
