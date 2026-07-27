using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{ 
    public bool isOn = false;

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
        }
        else
        {
            newLight.enabled = false;
            isOn = false;
        }
        flashlightAudioSource.PlayOneShot(flashlightAudioClip);
    }
}
