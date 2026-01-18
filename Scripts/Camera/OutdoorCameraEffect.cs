using UnityEngine;

public class OutdoorCameraEffect : MonoBehaviour
{
    #region MISC
    [Header("MISC")]
    private Vector3 initialPos;

    private readonly float movementSpeed = 1f;
    private readonly float movementRange = 0.5f;
    #endregion

    #region CAMERAS
    [Header("CAMERAS")]
    [SerializeField] private Camera secondaryCamera;

    [SerializeField] private GameObject mainCameraObj;
    [SerializeField] private GameObject secondaryCameraObj;
    #endregion

    public GameObject MainCameraObj => mainCameraObj;
    public GameObject SecondaryCameraObj => secondaryCameraObj;
    private void Start()
    {
        secondaryCameraObj.SetActive(true);
        mainCameraObj.SetActive(false);
        initialPos = secondaryCamera.transform.position;
    }

    private void Update()
    {
        OutdoorCameraMovingEffect();
    }

    public void OutdoorCameraMovingEffect()
    {
        if (secondaryCamera.enabled)
        {
            float offsetX = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetY = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            float offsetZ = Mathf.Sin(Time.time * movementSpeed) * movementRange;
            secondaryCamera.transform.position = initialPos + new Vector3(-offsetX, -offsetY, offsetZ);
        }
    }
}
