using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ShareFile : MonoBehaviour
{
    public GameObject ShareGO;
    public InputField ShareInput;
    public InputField ShareLink;
    public InputField LoadLink;
    public string loadExternal;
    public GameObject LoadGO;

    public void Share()
    {
        ShareGO.SetActive(true);
    }

    public void ShareClicked()
    {
        ShareGO.SetActive(false);
    }

    public void LoadExternal()
    {
        loadExternal = LoadLink.text;
        StartCoroutine(LoadFromLink());
        LoadGO.SetActive(false);
    }

    private void Update()
    {
        string userName = PlayerPrefs.GetString("UserName");
        ShareLink.text = "https://www.syncorsol.com/Syncor/" + userName + ShareInput.text + ".json";
    }

    IEnumerator LoadFromLink()
    {
        string test = System.IO.Path.Combine(Application.persistentDataPath, "loadexternal.json");
        string url = loadExternal;

        var file = new WWW(url);
        yield return file;
        File.WriteAllBytes(test, file.bytes);
        yield return file;
        string uploadURL = "https://www.syncorsol.com/Syncor/upload.php";

        SaveData.Load(test);

        StartCoroutine(UploadFileCo(test, uploadURL));
    }

    IEnumerator UploadFileCo(string localFileName, string uploadURL)
    {
        WWW localFile = new WWW("file:///" + localFileName);
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
        postForm.AddBinaryData("theFile", localFile.bytes, localFileName, "text/plain");
        WWW upload = new WWW(uploadURL, postForm);
        yield return upload;
        if (upload.error == null)
            Debug.Log("upload done :" + upload.text);
        else
            Debug.Log("Error during upload: " + upload.error);
    }
}
