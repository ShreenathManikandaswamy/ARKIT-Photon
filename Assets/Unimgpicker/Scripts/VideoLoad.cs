using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class VideoLoad : MonoBehaviour
{
    public GameObject Video;
    //public Camera Cam;
    public GameObject UI;


    // Start is called before the first frame update
    void Start()
    {
        var VideoPlayer = Video.AddComponent<UnityEngine.Video.VideoPlayer>();
        VideoPlayer.playOnAwake = false;
        VideoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
        //VideoPlayer.targetCameraAlpha = 0.5f;
        //VideoPlayer.targetCamera = Cam;
        VideoPlayer.url = System.IO.Path.Combine(Application.persistentDataPath, "testvideo.mp4");
        VideoPlayer.isLooping = true;
        //VideoPlayer.Play();
    }

    public void playVideo()
    {
        Video.SetActive(true);
        UI.SetActive(false);
        Video.GetComponent<UnityEngine.Video.VideoPlayer>().Play();
    }
}
