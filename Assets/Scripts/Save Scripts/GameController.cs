using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameController : Photon.MonoBehaviour
{
    public GameObject SaveFile;
    public InputField SaveWorkSpaceName;

    public GameObject LoadFile;
    public InputField LoadWorkSpaceName;

    //When using testing scene use this.
    //public const string playerPath = "TestPrefabGreen";
    //public const string playerPath2 = "TestPrefabYellow";

    //When using ARCore Scene Use this
    public const string playerPath = "Cube";
    //public const string playerPath2 = "YellowAndyPrefab";

    private static string savedatapath = string.Empty;
    private static string loaddatapath = string.Empty;


    private void Awake()
    {
        //datapath = System.IO.Path.Combine(Application.persistentDataPath, "Actors.json");
        //datapath = System.IO.Path.Combine(Application.persistentDataPath, SaveFile.name + ".json");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static Actor CreateActor (string path, Vector3 position, Quaternion rotation)
    {
            GameObject prefab = Resources.Load<GameObject>(path);

            GameObject go = PhotonNetwork.Instantiate(prefab.name, position, rotation, 0) as GameObject;

            Actor actor = go.GetComponent<Actor>() ?? go.AddComponent<Actor>();

            return actor;
    }

    public static Actor CreateActor(ActorData data, string path, Vector3 position, Quaternion rotation)
    {
        Actor actor = CreateActor(path, position, rotation);

        actor.data = data;

        return actor;
    }

    public void Save()
    {
        Debug.Log("Save Button Clicked");
        SaveFile.SetActive(true);
        LoadFile.SetActive(false);
    }

    public void Load()
    {
        LoadFile.SetActive(true);
        SaveFile.SetActive(false);
        //SaveData.Load(datapath);
    }

    public void SaveInto()
    {
        Debug.Log(SaveWorkSpaceName.text);   
        SaveFile.SetActive(false);
        string userName = PlayerPrefs.GetString("UserName");
        Debug.Log(userName);
        savedatapath = System.IO.Path.Combine(Application.persistentDataPath, userName + SaveWorkSpaceName.text.ToString() + ".json");
        //string m_URL = "http://localhost:8888/Syncor/upload.php";
        string m_URL = "https://www.syncorsol.com/Syncor/upload.php";
        SaveData.Save(savedatapath, SaveData.actorContainer);
        StartCoroutine(UploadFileCo(savedatapath, m_URL));
        Debug.Log("File Saved");
    }

    public void LoadFrom()
    {
        LoadFile.SetActive(false);
        loaddatapath = System.IO.Path.Combine(Application.persistentDataPath, LoadWorkSpaceName.text.ToString() + ".json");
        SaveData.Load(loaddatapath);
        Debug.Log("File Loaded");
    }

    IEnumerator UploadFileCo(string localFileName, string uploadURL)
    {
        Debug.Log("LocalFileName " + localFileName);
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
