using UnityEngine;

public class KidsRoomLight : MonoBehaviour
{
    #region LIGHTING
    [Header("LIGHTING")]
    [SerializeField] private Light kidsRoomLight;
    #endregion

    private void Start()
    {
        kidsRoomLight = GetComponent<Light>();
        kidsRoomLight.enabled = false;
    }
}
