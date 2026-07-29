using System.Collections;
using UnityEngine;

public class DoorBase : MonoBehaviour, IInteractable
{
    public string openParameter;
    public string closeParameter;
    public string idleParameter;

    private bool canInteract = true;

    private float interactioDelay = 1f;

    [SerializeField] private DoorStates currentDoorState;
    [SerializeField] private DoorLockState currentDoorLockState = DoorLockState.Locked;

    #region SERVICES
    private AudioManager audioManager;
    #endregion

    #region ANIMATIONS
    [Header("ANIMATIONS")]
    public Animator doorAnimator;
    #endregion

    #region PROPERTIES
    public bool IsLocked => currentDoorLockState == DoorLockState.Locked;
    #endregion
    public void Start()
    {
        audioManager = ServiceManager.GetService<AudioManager>();

        currentDoorState = DoorStates.isIdle;

        doorAnimator.SetBool(openParameter, false);
        doorAnimator.SetBool(closeParameter, false);
        doorAnimator.SetBool(idleParameter, true);
    }

    public void Interact(Interactor interactor)
    {
        interactor.HandleInteractableGameObject(this);
    }

    public void Unlock()
    {
        currentDoorLockState = DoorLockState.Unlocked;
    }

    public virtual void OnDoorInteract()
    {
        if (!canInteract)
            return;

        if (IsLocked)
            return;

        canInteract = false;

        if (currentDoorState == DoorStates.isIdle || currentDoorState == DoorStates.isClosed)
            OpenDoor();
        else if (currentDoorState == DoorStates.isOpened)
            CloseDoor();

        StartCoroutine(InteractionDelay());
    }

    public void OpenDoor()
    {
        doorAnimator.SetBool(openParameter, true);
        doorAnimator.SetBool(closeParameter, false);
        doorAnimator.SetBool(idleParameter, false);

        AttachAndPlayOpenDoorAudioSource();

        currentDoorState = DoorStates.isOpened;
    }

    public void CloseDoor()
    {
        doorAnimator.SetBool(openParameter, false);
        doorAnimator.SetBool(closeParameter, true);
        doorAnimator.SetBool(idleParameter, false);

        AttachAndPlayCloseDoorAudioSource();

        currentDoorState = DoorStates.isClosed;
    }

    public void AttachAndPlayCloseDoorAudioSource()
    {
        audioManager.DoorClosed.source.transform.SetParent(transform, true);
        audioManager.DoorClosed.source.transform.localPosition = Vector3.zero;

        audioManager.DoorClosed.source.transform.position = 
            audioManager.TriggerInteractable3DMusic.transform.position;

        audioManager.PlaySFX(audioManager.DoorClosed.source, audioManager.DoorClosed.clip);
    }

    public void AttachAndPlayOpenDoorAudioSource()
    {
        audioManager.DoorOpened.source.transform.SetParent(transform, true);
        audioManager.DoorOpened.source.transform.localPosition = Vector3.zero;

        audioManager.DoorOpened.source.transform.position = 
            audioManager.TriggerInteractable3DMusic.transform.position;

        audioManager.PlaySFX(audioManager.DoorOpened.source, audioManager.DoorOpened.clip);
    }

    private IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(interactioDelay);
        canInteract = true;
        Debug.Log("Can interact with the door again.");
    }
}
