using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{ 
    public bool isOn = false;

    [Header("UI")]
    [SerializeField] private Image flashlightImage;

    [Header("GAME OBJECTS")]
    [SerializeField] private GameObject flashlight;
    public GameObject flashlightAudioSourceObj;

    [Header("AUDIO")]
    [SerializeField] private AudioSource flashlightAudioSource;
    [SerializeField] private AudioClip flashlightAudioClip;

    [Header("OTHER")]
    public Light newLight;

    public void Toggle()
    {
        if (newLight.enabled == false)
        {
            newLight.enabled = true;
            isOn = true;
            flashlightImage.color = Color.red;
        }
        else
        {
            newLight.enabled = false;
            isOn = false;
            flashlightImage.color = Color.white;
        }
        flashlightAudioSource.PlayOneShot(flashlightAudioClip);
    }
}
