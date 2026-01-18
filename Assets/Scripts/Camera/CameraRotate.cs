using TMPro;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour
{
    #region GENERAL
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float spotlightRotationSpeed = 15f;
    [SerializeField] private float mainCameraRotationSpeed = 5f;

    private float xRotation;
    private float yRotation;
    private float zRotation;

    private float spotlightYRotation;
    private float spotlightXRotation;

    private float mainCameraYRotation;
    private float mainCameraXRotation;

    [SerializeField] private Transform spotlight;
    [SerializeField] private Transform mainCamera;

    public float SensitivitySlider
    {
        get { return sensitivitySlider.value; }
        set { sensitivitySlider.value = value; }
    }
    #endregion

    #region SCRIPT REFERENCES
    [Header("SCRIPT REFERENCES")]
    [SerializeField] private PlayerRotate playerRotate;
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;
    #endregion

    private void Start()
    {
        xRotation = 0f;
        yRotation = -90f;
        zRotation = 0f;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

        spotlightYRotation = yRotation;
        spotlightXRotation = xRotation;

        mainCameraYRotation = yRotation;
        mainCameraXRotation = xRotation;
    }

    private void LateUpdate()
    {
        if (RoundManager.Instance.CurrentMenuState == MenuState.OnInventoryMenu) return;

        if (RoundManager.Instance.CurrentMenuState == MenuState.OnNoteMenu) return;

        if (RoundManager.Instance.CurrentGameState == GameState.OnPlaying)
        {
            float mouseY = Input.GetAxis("Mouse Y") * sensitivitySlider.value;
            float mouseX = Input.GetAxis("Mouse X") * sensitivitySlider.value;

            xRotation -= mouseY;
            yRotation += mouseX;

            xRotation = Mathf.Clamp(xRotation, minX, maxX);
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            playerRotate.RotatePlayer(yRotation);

            UpdateSpotlightRotation(xRotation, yRotation);
            UpdateCameraRotation(xRotation, yRotation);
        }
    }

    public void UpdateSpotlightRotation(float newXRotation, float newYRotation)
    {
        spotlightYRotation = Mathf.Lerp(spotlightYRotation, newYRotation, spotlightRotationSpeed * Time.deltaTime);
        spotlightXRotation = Mathf.Lerp(spotlightXRotation, newXRotation, spotlightRotationSpeed * Time.deltaTime);
        spotlight.rotation = Quaternion.Euler(spotlightXRotation, spotlightYRotation, zRotation);
    }

    public void UpdateCameraRotation(float newXRotation, float newYRotation)
    {
        mainCameraYRotation = Mathf.Lerp(mainCameraYRotation, newYRotation, mainCameraRotationSpeed * Time.deltaTime);
        mainCameraXRotation = Mathf.Lerp(mainCameraXRotation, newXRotation, mainCameraRotationSpeed * Time.deltaTime);
        mainCamera.rotation = Quaternion.Euler(mainCameraXRotation, mainCameraYRotation, zRotation);
    }

    public void SetInitialRotation(float initialYRotation)
    {
        yRotation = initialYRotation;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
    }

    public void OnSensitivityChanged()
    {
        float sliderValue = sensitivitySlider.value;
        sensitivityValueText.text = sliderValue.ToString("0%");
        PlayerPrefs.SetFloat("SensValue", sliderValue);
        PlayerPrefs.Save();
    }
}
