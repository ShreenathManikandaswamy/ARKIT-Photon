using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using MiniJSON;
using System.Net;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;

public class ServerStateManager : MonoBehaviour
{
    public void UpdateGameState(string url,object queryObj = null, Action<object> actionCallBack = null)
    {

        //string query = ConstructQuery(queryData);
      

        StartCoroutine(ServerAPIHandler.Instance.webRequestHandler.Request(url, WEB_REQUEST_TYPE.GRAPH_QUERY, null, null, queryObj,(WebRequestInfo status) =>
        {
            if (status.isSuccess)
            {
               
               // if (status.responseCode.Equals("200"))
                {
                    if (actionCallBack != null)
                    {
                        actionCallBack(true);
                    }
                }

            }
            else
            {

                if (actionCallBack != null)
                {
                    actionCallBack(status);
                }
            }

        }));
    }

    void OnGameStateUpdateSuccess(string responceDataString,Action<object> actionCallBack = null)
    {
       // bool updateResponceSuccess = false;
        Dictionary<string, object> _responceCode = new Dictionary<string, object>();

        foreach (KeyValuePair<string, object> statusResponceKeypair in Json.Deserialize(responceDataString) as Dictionary<string, object>)
        {
            if (statusResponceKeypair.Key.Equals("status"))
            {
                if (statusResponceKeypair.Value.ToString().Equals("200")) // Update Success
                {
                 //   updateResponceSuccess = true;
                    if (actionCallBack != null)
                {
                    actionCallBack(true);
                }
                }
            }
            //else if (queryData.statusResponseString.Contains(statusResponceKeypair.Key))
            //{
            //    if (statusResponceKeypair.Value == null)
            //    {
            //        if (actionCallBack != null)
            //        {
            //            actionCallBack(false);
            //        }
                   
            //    }
            //    else
            //    {
            //        _responceCode.Add(statusResponceKeypair.Key, statusResponceKeypair.Value);
            //    }
            //}
        }

        //if (updateResponceSuccess)
        //{

        //    if (queryData.statusResponseString.Equals(string.Empty))
        //    {
        //        if (actionCallBack != null)
        //        {
        //            actionCallBack(true);
        //        }
        //    }
        //    else
        //    {
        //        if (actionCallBack != null)
        //        {
        //            actionCallBack(_responceCode);
        //        }
        //    }

        //}
        //else
        //{
        //    if (actionCallBack != null)
        //    {
        //        actionCallBack(false);
        //    }
        //}
    }

   
    string ConstructQuery(QueryData queryData)
    {
       // string updateQueryHeader = GetRequestUpdateType(queryData.requestUpdateType);

        Dictionary<string, object> keyValuePairs = queryData.keyValuePairs;

        string updateString = string.Empty;

        if (keyValuePairs.Keys != null)
        {
            foreach (var key in keyValuePairs.Keys)
            {
                object keyValue = keyValuePairs[key];

                if (!updateString.Equals(string.Empty))
                {
                    updateString += ",";
                }

                if (keyValue.GetType().Equals(typeof(int)))
                {
                    updateString += ServerConstants.quotationMark + key + ServerConstants.quotationMark + ":" + (int)keyValue;
                }
                else if (keyValue.GetType().Equals(typeof(bool)))
                {
                    updateString += ServerConstants.quotationMark + key+ServerConstants.quotationMark  + ":" + ((bool)keyValue);
                }
                else
                {
                    updateString += ServerConstants.quotationMark + key+ ServerConstants.quotationMark + ":" + ServerConstants.quotationMark + keyValue + ServerConstants.quotationMark;
                }
            }
        }

        string statsString = "}";//"){status}}";

        //if (queryData.statusResponseString.Count > 0)
        //{
        //    string statusResponce = string.Empty;

        //    for (int i = 0; i < queryData.statusResponseString.Count; i++)
        //    {
        //        if (i != 0)
        //        {
        //            statusResponce += ",";
        //        }
        //        statusResponce += queryData.statusResponseString[i];
        //    }

        //    statsString = "){status," + statusResponce + "}}";
        //}

        // return updateQueryHeader + updateString + statsString;
        return "{" + updateString + statsString;

    }

    //string GetRequestUpdateType(SERVER_UPDATE_REQUEST_TYPE uPDATE_TYPE)
    //{
    //    string query = string.Empty;

    //    switch (uPDATE_TYPE)
    //    {
    //        case SERVER_UPDATE_REQUEST_TYPE.LOGIN:
    //            query = ServerConstants.LOGIN_REQUEST_QUERY;
    //            break;
    //        case SERVER_UPDATE_REQUEST_TYPE.RIGISTERER:
    //            query = ServerConstants.REGISTER_REQUEST_QUERY;
    //            break;
    //    }

    //    return query;
    //}


}

public class QueryData
{
    public SERVER_UPDATE_REQUEST_TYPE requestUpdateType;
    public Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
    public List<string> statusResponseString = new List<string>();
    public WWWForm webFormData;
}

public class Register
{
    public string username ;
    public string password ;
    public string forename ;
    public string surname ;
    public string schoolname ;
    public long country;
    public long state;
    public long city;
}

public class Login
{
    public string username;
    public string password;
}

public class ForgorPassword
{
    public string username;
}



