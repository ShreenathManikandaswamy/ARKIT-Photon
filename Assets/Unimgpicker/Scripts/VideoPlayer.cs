using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.UI;

namespace Kakera
{
    public class VideoPlayer : MonoBehaviour
    {

        private string uploadURL = "http://localhost:8888/Syncor/upload.php";
        //private string uploadtoURL = "http://localhost:8888/Syncor/";
        //private string video = "http://localhost:8888/Syncor/Nature%20Beautiful%20short%20video%20720p%20HD.mp4";
        private string video = "http://syncorsol.com/test/testvideo.mp4";

        public GameObject Ui;
        public Text number;
        public Text Uploading;


        [SerializeField]
        private Unimgpicker imagePicker;

        [SerializeField]
        private Renderer VideoRenderer;
        //private RawImage imageRenderer;

       /* void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, VideoRenderer));
            };
        }*/

        /*private void Start()
        {
            Ui.SetActive(true); 
        }*/

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "VideoPicker", 1000000);
            //string path = System.IO.Path.Combine(Application.dataPath, "Nature Beautiful short video 720p HD.mp4");
            string path = System.IO.Path.Combine(Application.dataPath, "testvideo.mp4");
            StartCoroutine(LoadVideo(path, VideoRenderer));
            Ui.SetActive(true);
        }


        private IEnumerator LoadVideo(string path, Renderer output)
        {
            //string test = System.IO.Path.Combine(Application.persistentDataPath, "Nature Beautiful short video 720p HD.mp4");
            //string url = "http://localhost:8888/Syncor/Nature%20Beautiful%20short%20video%20720p%20HD.mp4";

            string test = System.IO.Path.Combine(Application.persistentDataPath, "testvideo.mp4");
            string url = "http://syncorsol.com/test/testvideo.mp4";

            //var url = "file://" + path;
            var video = new WWW(url);
            number.text = "1 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "2 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "3 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "4 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "5 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "6 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "7 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "8 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "9 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "10 / 10".ToString();
            yield return new WaitForSeconds(1);
            number.text = "Done!!";
            yield return new WaitForSeconds(1);
            yield return video;
            File.WriteAllBytes(test, video.bytes);
            yield return video;

            //StartCoroutine(UploadFileCo(url, uploadURL));

        }


        /*IEnumerator UploadFileCo(string localFileName, string uploadURL)
        {
            WWW localFile = new WWW(localFileName);
            Debug.Log("LocalFileName = " + localFileName);
            yield return localFile;
            if (localFile.error == null)
                Debug.Log("Loaded file successfully");
            else
            {
                Debug.Log("Open file error: " + localFile.error);
                yield break; // stop the coroutine here
            }
            WWWForm postForm = new WWWForm();
            // version 1
            //postForm.AddBinaryData("theFile",localFile.bytes);
            // version 2
            postForm.AddBinaryData("theFile", localFile.bytes, localFileName, ".jpg/.png/.jpeg");
            WWW upload = new WWW(uploadURL, postForm);
            yield return upload;
            if (upload.error == null)
                Debug.Log("upload done :" + upload.text);
            else
                Debug.Log("Error during upload: " + upload.error);
        }*/


    }
}