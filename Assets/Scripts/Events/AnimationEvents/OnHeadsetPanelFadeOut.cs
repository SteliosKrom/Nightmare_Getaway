using UnityEngine;

public class OnHeadsetPanelFadeOut : MonoBehaviour
{
    private Transition transition;

    private void Awake()
    {
        transition = GameObject.Find("Transition").GetComponent<Transition>();
    }

    public void OnHeadsetPanelFadeOutMethod()
    {
        transition.LoadGame();
    }
}
