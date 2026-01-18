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

    [Header("AUDIO")]
    [SerializeField] private AudioSource doorOpenedAudioSource;
    [SerializeField] private AudioSource doorClosedAudioSource;

    [SerializeField] private AudioClip doorOpenedAudioClip;
    [SerializeField] private AudioClip doorClosedAudioClip;

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
            AudioManager.Instance.PlaySFX(doorOpenedAudioSource, doorOpenedAudioClip);
        }
        else if (currentDoorState == DoorStates.isOpened)
        {
            CloseDoor();
            AudioManager.Instance.PlaySFX(doorClosedAudioSource, doorClosedAudioClip);
        }
        StartCoroutine(InteractionDelay());
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool(openParameter, true);
        doorAnimator.SetBool(closeParameter, false);
        doorAnimator.SetBool(idleParameter, false);
        currentDoorState = DoorStates.isOpened;
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool(openParameter, false);
        doorAnimator.SetBool(closeParameter, true);
        doorAnimator.SetBool(idleParameter, false);
        currentDoorState = DoorStates.isClosed;
    }

    private IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(interactioDelay);
        canInteract = true;
        Debug.Log("Can interact with the door again.");
    }
}
