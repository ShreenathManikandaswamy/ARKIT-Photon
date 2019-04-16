using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public GameObject PrimaryPanel;
    public GameObject SecondaryPanel;
    public GameObject FirstArrow;
    public GameObject SecondArrow;
    public GameObject ThirdArrow;
    public GameObject EmbedPanel;
    public GameObject PickerControler;
    public GameObject ScreenShotButton;


    private void Start()
    {
        FirstArrow.SetActive(true);
        PrimaryPanel.SetActive(false);
        SecondaryPanel.SetActive(false);
        SecondArrow.SetActive(false);
        ThirdArrow.SetActive(false);
        EmbedPanel.SetActive(false);
        PickerControler.SetActive(false);
        ScreenShotButton.SetActive(true);
    }

    public void ScreenShotCapture()
    {
        ScreenShotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
        NativeToolkit.SaveScreenshot("MyScreenshot", "MyScreenshotFolder", "jpeg");
    }

    public void OnClickEmbedPanel()
    {
        FirstArrow.SetActive(false);
        PrimaryPanel.SetActive(true);
        SecondaryPanel.SetActive(false);
        SecondArrow.SetActive(false);
        ThirdArrow.SetActive(true);
        EmbedPanel.SetActive(true);
        PickerControler.SetActive(true);
    }

    public void Primary()
    {
        FirstArrow.SetActive(false);
        PrimaryPanel.SetActive(true);
        SecondaryPanel.SetActive(false);
        SecondArrow.SetActive(true);
        ThirdArrow.SetActive(false);
        EmbedPanel.SetActive(false);
        ScreenShotButton.SetActive(false);
    }

    public void Secondary()
    {
        FirstArrow.SetActive(true);
        PrimaryPanel.SetActive(false);
        SecondaryPanel.SetActive(false);
        SecondArrow.SetActive(false);
        ThirdArrow.SetActive(false);
        EmbedPanel.SetActive(false);
        ScreenShotButton.SetActive(true);
    }

    public void Thrid()
    {
        FirstArrow.SetActive(false);
        PrimaryPanel.SetActive(true);
        SecondaryPanel.SetActive(false);
        SecondArrow.SetActive(true);
        ThirdArrow.SetActive(false);
        EmbedPanel.SetActive(false);
    }

    public void SecondayPanelTrigger()
    {
        FirstArrow.SetActive(false);
        PrimaryPanel.SetActive(true);
        SecondaryPanel.SetActive(true);
        SecondArrow.SetActive(false);
        ThirdArrow.SetActive(true);
        EmbedPanel.SetActive(false);
    }
}
