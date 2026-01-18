using UnityEngine;
using TMPro;

public class FPScounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    public GameObject fps;

    public int frameCount = 0;
    public float elapsedTime = 0f;
    private const float updateInterval = 1f;

    void Update()
    {
        CountFPS();
    }

    public void CountFPS()
    {
        switch (RoundManager.Instance.CurrentGameState)
        {
            case GameState.OnPlaying:
                FPSCalculation();
                break;
            case GameState.OnPause:
                PauseFPSCalculation();
                break;
        }

        switch (RoundManager.Instance.CurrentMenuState)
        {
            case MenuState.OnMainMenu:
            case MenuState.OnGameSettings:
            case MenuState.OnMenuSettings:
                FPSCalculation();
                break;
        }
    }

    public void FPSCalculation()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= updateInterval)
        {
            float fps = frameCount / elapsedTime;
            fpsText.text = "" + Mathf.Ceil(fps).ToString();

            frameCount = 0;
            elapsedTime = 0f;
        }
    }

    public void PauseFPSCalculation()
    {
        frameCount = 0;
        elapsedTime = 0f;
        fpsText.text = "0";
    }
}
