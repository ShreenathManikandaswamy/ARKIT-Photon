using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using MiniJSON;
using System.Security.Cryptography;
public class Test : MonoBehaviour
{

    // Update is called once per frame
    //void Update()
    //{
    //    if(Input.GetKeyUp(KeyCode.I))
    //    {
    //     string query =""
    //    }
    //}
   

    public IEnumerator call(string query, string _type)
    {
        switch (_type)
        {
            case "1":

                object fullQuery = new GraphQLQuery()
                {
                    //payload = query,
                };

                string jsonString = JsonUtility.ToJson(fullQuery);
                Debug.Log("jsonString:" + jsonString);
                byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

                using (UnityWebRequest www = new UnityWebRequest(ServerConstants.SERVER_GAME_STATE_URL))
                {
                    www.method = UnityWebRequest.kHttpVerbPOST;
                    www.uploadHandler = new UploadHandlerRaw(bytes);
                    www.downloadHandler = new DownloadHandlerBuffer();
                    www.uploadHandler.contentType = "application/json";
                    www.chunkedTransfer = false;

                    yield return www.SendWebRequest();

                    if (www.isNetworkError || www.isHttpError)
                    {
                        Debug.Log(www.responseCode);
                        Debug.Log(www.error);
                    }
                    else
                    {
                        Debug.Log("Post complete! RespLength:" + www.downloadHandler.text.Length);
                        Debug.Log("text:" + www.downloadHandler.text);
                    }

                }
                break;

        }


    }

}

