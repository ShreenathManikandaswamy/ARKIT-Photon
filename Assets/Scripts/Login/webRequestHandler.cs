using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using MiniJSON;
using System.Security.Cryptography;

public class webRequestHandler : MonoBehaviour {

    // Don't declare local variables
    public IEnumerator Request(string url,
                               WEB_REQUEST_TYPE webRequestMethodType,
                               WWWForm wWWForm = null,
                               string query = null,
                               object queryObj = null,
                               Action<WebRequestInfo> callBackWithData = null
                               )
    {

     
        {
            switch (webRequestMethodType)
            {
                case WEB_REQUEST_TYPE.POST:
                    {
                        using (UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wWWForm))
                        {

                            yield return unityWebRequest.SendWebRequest();

                            WebRequestInfo info = new WebRequestInfo();


                            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                            {
                                info.errorDescription = unityWebRequest.error;
                                info.isSuccess = false;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(unityWebRequest.error) && unityWebRequest.isDone)
                                {
                                    info.callBackData = unityWebRequest.downloadHandler.data;

                                    //string responseText = Encoding.UTF8.GetString(info.callBackData);

                                    info.isSuccess = true;
                                }
                                else
                                {
                                    info.errorDescription = unityWebRequest.error;
                                    info.isSuccess = false;
                                }
                            }
                            yield return new WaitForEndOfFrame();
                            callBackWithData(info);
                        }
                    }
                    break;

                case WEB_REQUEST_TYPE.GET:
                    {
                        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
                        {
                            unityWebRequest.timeout = (int)ServerConstants.REQUEST_TIMEOUT;

                            yield return unityWebRequest.SendWebRequest();

                            //Utilities.DebugLogColor("Data progress : " + unityWebRequest.downloadProgress, "red");

                            WebRequestInfo info = new WebRequestInfo();

                          //  InternetValidator.Instance.IsInternetAvailable = !unityWebRequest.isNetworkError;

                            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                            {
                                info.errorDescription = unityWebRequest.error;
                                info.isSuccess = false;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(unityWebRequest.error) && unityWebRequest.isDone)
                                {
                                    if (string.IsNullOrEmpty(unityWebRequest.downloadHandler.text))
                                    {
                                        info.errorDescription = "Content not available";
                                        info.isSuccess = false;
                                    }
                                    else
                                    {
                                        info.isSuccess = true;
                                        info.callBackData = unityWebRequest.downloadHandler.data;

                                        //string responseText = Encoding.UTF8.GetString(info.callBackData);

                                    }
                                }
                                else
                                {
                                    info.errorDescription = unityWebRequest.error;
                                    info.isSuccess = false;
                                }
                            }
                            yield return new WaitForEndOfFrame();
                            callBackWithData(info);
                        }
                    }
                    break;

                case WEB_REQUEST_TYPE.GRAPH_QUERY:
                    {
                        string jsonString = JsonUtility.ToJson(queryObj);

                        Debug.Log("jsonString:" + jsonString);

                         byte[] bytes = Encoding.UTF8.GetBytes(jsonString);

                        using (UnityWebRequest www = new UnityWebRequest(url))
                        {
                            www.method = UnityWebRequest.kHttpVerbPOST;
                            www.uploadHandler = new UploadHandlerRaw(bytes);
                            www.downloadHandler = new DownloadHandlerBuffer();
                            www.uploadHandler.contentType = "application/json";
                            www.chunkedTransfer = false;

                            yield return www.SendWebRequest();

                            WebRequestInfo info = new WebRequestInfo();

                            string callbackDataInString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                            Debug.Log(callbackDataInString);
                            Dictionary<string, object> response = Json.Deserialize(callbackDataInString) as Dictionary<string, object>;


                            if (www.isNetworkError || www.isHttpError)
                            {
                                Debug.Log("responseCode:"+www.responseCode);
                                Debug.Log(www.error);

                                Debug.Log(www.downloadHandler.data);
                                info.responseCode = www.responseCode.ToString();
                                info.responceDataString = www.downloadHandler.text;
                                info.isSuccess = false;

                                if (response != null && response.ContainsKey("errorMessage"))
                                {
                                    info.errorDescription = response["errorMessage"].ToString();
                                }
                                else {
                                    info.errorDescription = "Unknown error";
                                }
                            }
                            else
                            {
                                Debug.Log("Post complete! RespLength:" + www.downloadHandler.text.Length);
                                Debug.Log("text:" + www.downloadHandler.text);

                                if (String.IsNullOrEmpty(www.error) && www.isDone)
                                {
                                    //string callbackDataInString = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                                    //Debug.Log(callbackDataInString);
                                    //Dictionary<string, object> response = Json.Deserialize(callbackDataInString) as Dictionary<string, object>;
                                    if (response.ContainsKey("data"))
                                    {
                                        foreach (KeyValuePair<string, object> author in response)
                                        {
                                            if (author.Key.Contains("errors"))
                                            {
                                                info.errorDescription = "Data Not Available";
                                                info.isSuccess = false;
                                                break;
                                            }


                                            info.responseObj = author.Value;
                                            foreach (KeyValuePair<string, object> responceKeypair in author.Value as Dictionary<string, object>)
                                            {

                                                string responceValueString = Json.Serialize(responceKeypair.Value as Dictionary<string, object>);
                                                if (!string.IsNullOrEmpty(responceValueString))
                                                {
                                                    info.responceDataString = responceValueString;
                                                    info.isSuccess = true;
                                                }
                                                else
                                                {
                                                    info.errorDescription = "Data Not Available";
                                                    info.responseCode = www.responseCode.ToString();
                                                    info.isSuccess = false;
                                                }
                                            }


                                        }


                                    }
                                    else
                                    {
                                        info.responceDataString = callbackDataInString;
                                        info.isSuccess = true;
                                    }
                                }
                                else
                                {
                                    info.errorDescription = www.error;
                                    info.isSuccess = false;
                                }
                            }
                            yield return new WaitForEndOfFrame();
                            callBackWithData(info);
                        }

                    }
                    break;
                }
        }
    }
    public string GetNonce()
    {
        string uuid = Guid.NewGuid().ToString();
        return uuid;
    }

    public string CalculateMD5Hash(string inputString)
    {
        StringBuilder hash = new StringBuilder();
        MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
        byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(inputString));

        for (int i = 0; i < bytes.Length; i++)
        {
            hash.Append(bytes[i].ToString("x2")); //lowerCase; X2 if uppercase desired
        }
        return hash.ToString();
    }
}

public class WebRequestInfo
{
    public bool isSuccess = false;
    public bool isInterNetConnectionAvailable = true;
    public byte[] callBackData = null;
    public string errorDescription = String.Empty;
    public string responceDataString = null;
    public string responseCode = null;
    public object responseObj = null;
    public bool isServerDown = false;
}

[System.Serializable]
public class GraphQLQuery
{
   // public string payload;

    public string username ="abcd";
    public string password = "fgh";
    public string forename = "wer";
    public string surname = "rtr";
    public string schoolname = "nnk";
    public long country = 5;
    public long state = 1;
    public long city = 2;







}
