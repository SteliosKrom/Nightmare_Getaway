using System.Collections;
using System.Linq;
using UnityEngine;

public partial class Interactor
{
    private bool lockedCoroutineIsRunning = false;

    private string[] doorTags =
    {
        "KidsDoor, BathroomDoor, SecondBathroomDoor, SecondBedroomDoor, GarageDoor, BedroomDoor, ClothingsDoor"
    };

    private IEnumerator ToggleDelay()
    {
        canToggle = false;
        yield return new WaitForSeconds(toggleDelay);
        canToggle = true;
    }

    public IEnumerator GarageLightBreakDelay()
    {
        yield return new WaitForSeconds(2);
        garageRoomLight.enabled = false;
    }

    public IEnumerator HeartbeatAudioDelay()
    {
        yield return new WaitForSeconds(heartbeatAudioDelay);
        audioManager.Play(audioManager.HeartbeatAudioSource);
    }

    public IEnumerator PhoneCallDelay()
    {
        yield return new WaitForSeconds(telephoneCallDelay);
        telephoneAudioSource.Play();
        telephoneAudioLowPassFilter.cutoffFrequency = 1000f;
    }

    public IEnumerator LockedUIDelay()
    {
        if (lockedCoroutineIsRunning) yield break;

        lockedCoroutineIsRunning = true;
        lockedMessagePanel.SetActive(true);

        audioManager.LockedDoor.source.transform.position = audioManager.TriggerInteractable3DMusic.transform.position;
        audioManager.PlaySFX(audioManager.LockedDoor.source, audioManager.LockedDoor.clip);

        yield return new WaitForSeconds(lockedUIDelay);

        lockedMessagePanel.SetActive(false);
        lockedCoroutineIsRunning = false;
    }

    public IEnumerator DoorCollidersDelay(BoxCollider collider)
    {
        if (doorTags.Contains(collider.tag))
        {
            collider.enabled = false;
            yield return new WaitForSeconds(doorCollidersDelay);
            collider.enabled = true;
        }
    }

    public IEnumerator doorHandleCollidersDelay(BoxCollider collider)
    {
        if (doorTags.Contains(collider.tag))
        {
            collider.enabled = false;
            yield return new WaitForSeconds(doorCollidersDelay);
            collider.enabled = true;
        }
    }
}
