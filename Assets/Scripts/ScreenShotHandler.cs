using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotHandler : MonoBehaviour
{
    private Camera MyCamera;

    private static ScreenShotHandler instance;

    private bool TakeScreenshotOnNextFrame;

    private string fileName = "ScreenShot.png";

    private void Awake()
    {
        instance = this;
        MyCamera = gameObject.GetComponent<Camera>();    
    }

    private void OnPostRender()
    {
        if(TakeScreenshotOnNextFrame)
        {
            TakeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = MyCamera.targetTexture;

            Texture2D RenderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            RenderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = RenderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + fileName, byteArray);


            Debug.Log("Saved CameraScreenShot");

            RenderTexture.ReleaseTemporary(renderTexture);
            MyCamera.targetTexture = null;
        }
    }

    void TakeScreenshot(int height, int width)
    {
        MyCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        TakeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height)
    {
        instance.TakeScreenshot(width, height);
    }
}
