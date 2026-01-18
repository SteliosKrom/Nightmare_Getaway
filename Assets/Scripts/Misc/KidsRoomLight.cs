using UnityEngine;

public class KidsRoomLight : MonoBehaviour
{
    [Header("OTHER")]
    [SerializeField] private Light kidsRoomLight;

    private void Start()
    {
        kidsRoomLight = GetComponent<Light>();
        kidsRoomLight.enabled = false;
    }
}
