using UnityEngine;

public class OtherInteractableSwitches : MonoBehaviour, IInteractable
{
    public void Interact(Interactor interactor)
    {
        interactor.HandleInteractableGameObject(this);
    }
}