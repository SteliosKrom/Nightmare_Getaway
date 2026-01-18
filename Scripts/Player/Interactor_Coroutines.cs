using System.Collections;
using UnityEngine;

public partial class Interactor
{
    private bool lockedCoroutineIsRunning = false;

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
        AudioManager.Instance.Play(AudioManager.Instance.HeartbeatAudioSource);
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

        isLocked = true;
        lockedMessagePanel.SetActive(true);

        AudioManager.Instance.LockedDoor.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.LockedDoor.source, AudioManager.Instance.LockedDoor.clip);

        yield return new WaitForSeconds(lockedUIDelay);

        isLocked = false;
        lockedMessagePanel.SetActive(false);

        lockedCoroutineIsRunning = false;
    }

    public IEnumerator DoorCollidersDelay(BoxCollider collider)
    {
        if (collider.CompareTag("KidsDoor")
            || collider.CompareTag("BathroomDoor") || collider.CompareTag("SecondBathroomDoor")
            || collider.CompareTag("SecondBedroomDoor") || collider.CompareTag("GarageDoor"))
        {
            collider.enabled = false;
            yield return new WaitForSeconds(doorCollidersDelay);
            collider.enabled = true;
        }
        else if (collider.CompareTag("BedroomDoor") || collider.CompareTag("ClothingsDoor"))
        {
            collider.enabled = false;
            yield return new WaitForSeconds(doorCollidersDelay);
            collider.enabled = true;
        }
    }

    public IEnumerator doorHandleCollidersDelay(BoxCollider collider)
    {
        if (collider.CompareTag("KidsDoor")
            || collider.CompareTag("BathroomDoor") || collider.CompareTag("SecondBathroomDoor")
            || collider.CompareTag("SecondBedroomDoor") || collider.CompareTag("GarageDoor"))
        {
            collider.enabled = false;
            yield return new WaitForSeconds(doorCollidersDelay);
            collider.enabled = true;
        }
        else if (collider.CompareTag("BedroomDoor") || collider.CompareTag("ClothingsDoor"))
        {
            collider.enabled = false;
            yield return new WaitForSeconds(doorCollidersDelay);
            collider.enabled = true;
        }
    }
}
