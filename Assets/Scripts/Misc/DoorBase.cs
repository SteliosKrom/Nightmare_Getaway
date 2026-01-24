using System.Collections;
using UnityEngine;

public enum DoorStates
{
    isOpened,
    isClosed,
    isIdle
}

public class DoorBase : MonoBehaviour, IInteractable
{
    public string openParameter;
    public string closeParameter;
    public string idleParameter;

    private bool canInteract = true;
    private bool isLocked = false;

    private float interactioDelay = 1f;

    public DoorStates currentDoorState;

    [Header("ANIMATIONS")]
    public Animator doorAnimator;

    public void Start()
    {
        currentDoorState = DoorStates.isIdle;
        doorAnimator.SetBool(openParameter, false);
        doorAnimator.SetBool(closeParameter, false);
        doorAnimator.SetBool(idleParameter, true);
    }

    public void Interact(Interactor interactor)
    {
        interactor.HandleInteractableGameObject(this);
    }

    public virtual void OnDoorInteract()
    {
        if (!canInteract)
        {
            return;
        }
        
        if (isLocked)
        {
            return;
        }
        canInteract = false;

        if (currentDoorState == DoorStates.isIdle || currentDoorState == DoorStates.isClosed)
        {
            OpenDoor();
        }
        else if (currentDoorState == DoorStates.isOpened)
        {
            CloseDoor();
        }
        StartCoroutine(InteractionDelay());
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool(openParameter, true);
        doorAnimator.SetBool(closeParameter, false);
        doorAnimator.SetBool(idleParameter, false);
        currentDoorState = DoorStates.isOpened;
        AttachAndPlayOpenDoorAudioSource();
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool(openParameter, false);
        doorAnimator.SetBool(closeParameter, true);
        doorAnimator.SetBool(idleParameter, false);
        currentDoorState = DoorStates.isClosed;
        AttachAndPlayCloseDoorAudioSource();
    }

    public void AttachAndPlayCloseDoorAudioSource()
    {
        AudioManager.Instance.DoorClosed.source.transform.SetParent(transform, true);
        AudioManager.Instance.DoorClosed.source.transform.localPosition = Vector3.zero;

        AudioManager.Instance.DoorClosed.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.DoorClosed.source, AudioManager.Instance.DoorClosed.clip);
    }

    public void AttachAndPlayOpenDoorAudioSource()
    {
        AudioManager.Instance.DoorOpened.source.transform.SetParent(transform, true);
        AudioManager.Instance.DoorOpened.source.transform.localPosition = Vector3.zero;

        AudioManager.Instance.DoorOpened.source.transform.position = AudioManager.Instance.TriggerInteractable3DMusic.transform.position;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.DoorOpened.source, AudioManager.Instance.DoorOpened.clip);
    }

    private IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(interactioDelay);
        canInteract = true;
        Debug.Log("Can interact with the door again.");
    }
}
