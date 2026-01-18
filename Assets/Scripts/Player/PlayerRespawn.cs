using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CameraRotate cameraRotate;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;
        
        if (characterController != null)
        {
            characterController.enabled = false;
            characterController.enabled = true;
        }

        if (cameraRotate != null)
        {
            cameraRotate.SetInitialRotation(-90f);
        }
    }
}
