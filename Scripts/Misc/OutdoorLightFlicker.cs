using UnityEngine;

public class OutdoorLightFlicker : MonoBehaviour
{
    #region LIGHTING
    [Header("LIGHTING")]
    private Light outdoorLight;
    #endregion

    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 5.0f;
    [SerializeField] private float flickerSpeed = 1.0f;

    private void Start()
    {
        outdoorLight = GetComponent<Light>();
        InvokeRepeating("Flicker", 0f, flickerSpeed);
    }

    public void Flicker()
    {
        float randomIntensity = Random.Range(minIntensity, maxIntensity);
        outdoorLight.intensity = randomIntensity;
    }
}
