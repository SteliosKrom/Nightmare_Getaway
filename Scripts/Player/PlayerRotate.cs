using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public void RotatePlayer(float yRotation)
    {
        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
