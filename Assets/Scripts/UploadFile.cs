// C# file names: "FileUpload.cs"
using UnityEngine;
using System.Collections;


public class UploadFile : MonoBehaviour
{
    
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
    public void FileUpload()
    {
        string userName = PlayerPrefs.GetString("UserFileName");
        string m_LocalFileName = System.IO.Path.Combine(Application.persistentDataPath + "/FabLabDoc/LocalDB/", userName + ".json");
        Debug.Log(m_LocalFileName);
        string m_URL = "http://localhost:8888/Syncor/upload.php";
        //string m_URL = "https://www.syncorsol.com/syncor/upload.php";
        StartCoroutine(UploadFileCo(m_LocalFileName, m_URL));
    }
}

