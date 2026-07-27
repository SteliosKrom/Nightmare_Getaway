using UnityEngine;
using TMPro;

public class FPScounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public GameObject fps;

    public float frameCount = 0;
    public float elapsedTime = 0f;
    private const float updateInterval = 1f;

    void Update()
    {
        CountFPS();
    }

    public void CountFPS()
    {
        frameCount = 1f / Time.unscaledDeltaTime;
        elapsedTime += Time.unscaledDeltaTime;

        if (elapsedTime >= updateInterval)
        {
            if (frameCount >= 999)
                fpsText.text = "999+";
            else
                fpsText.text = frameCount.ToString("0");

            elapsedTime = 0;
        }
    }
}
