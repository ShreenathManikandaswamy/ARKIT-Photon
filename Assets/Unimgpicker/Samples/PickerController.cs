using UnityEngine;
using System.Collections;
using System.IO;

namespace Kakera
{
    public class PickerController : MonoBehaviour
    {

        private string uploadURL = "http://localhost:8888/Syncor/upload.php";
        private string uploadtoURL = "http://localhost:8888/Syncor/";
        public GameObject ImageCube;

        [SerializeField]
        private Unimgpicker imagePicker;

        [SerializeField]
        private MeshRenderer imageRenderer;

        void Awake()
        {
            imagePicker.Completed += (string path) =>
            {
                StartCoroutine(LoadImage(path, imageRenderer));
            };
        }

        public void OnPressShowPicker()
        {
            imagePicker.Show("Select Image", "unimgpicker", 1024);
        }

       /*public void OnLoadLoadedImage()
        {
            string path = System.IO.Path.Combine(Application.persistentDataPath, "unimgpicker");
            StartCoroutine(LoadImage(path, imageRenderer));
        }*/

        public void LoadLoadedImage()
        {
            ImageCube.SetActive(true);
        }

        private IEnumerator LoadImage(string path, MeshRenderer output)
        {
            //ImageCube.SetActive(true);
            var url = "file://" + path;
            var www = new WWW(url);
            yield return www;

            var texture = www.texture;
            if (texture == null)
            {
                Debug.LogError("Failed to load texture url:" + url);
            }

            output.material.mainTexture = texture;

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