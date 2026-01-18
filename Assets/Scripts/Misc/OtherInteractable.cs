using UnityEngine;

public class OtherInteractable : MonoBehaviour, IInteractable
{
    public void Interact(Interactor interactor)
    {
        interactor.HandleInteractableGameObject(this);
    }
}