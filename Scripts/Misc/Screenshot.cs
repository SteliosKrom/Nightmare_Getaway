using System;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            string folderPath = UnityEngine.Application.dataPath + "/Screenshots";

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filename = $"Screenshot-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.png";
            string fullPath = Path.Combine(folderPath, filename);

            ScreenCapture.CaptureScreenshot(fullPath);
        }
    }
}
