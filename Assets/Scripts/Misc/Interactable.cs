using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public void Interact(Interactor interactor)
    {
        interactor.HandleInteractableGameObject(this);
    }
}