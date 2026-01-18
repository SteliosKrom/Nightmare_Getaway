using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private void Awake()
    {
        Resolution nativeRes = Screen.currentResolution;
        bool fullscreen = PlayerPrefs.GetInt("ScreenValue", 1) != 0;

        if (fullscreen)
        {
            Screen.SetResolution(nativeRes.width, nativeRes.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            int width = PlayerPrefs.GetInt("ScreenWidth", nativeRes.width / 2);
            int height = PlayerPrefs.GetInt("ScreenHeight", nativeRes.height / 2);
            Screen.SetResolution(width, height, FullScreenMode.Windowed);
        }
    }
}
