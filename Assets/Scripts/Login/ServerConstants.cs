﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerConstants
{


    #region COMMON_KEYS
    public static string USER_NAME = "username"; 
    public static string PASSWORD = "password";
    public static string FORENAME = "forename";
    public static string SURNAME = "surname";
    public static string SCHOOL_NAME = "schoolname";
    public static string COUNTRY = "country";
    public static string STATE = "state";
    public static string CITY = "city";
    #endregion

    #region GAME_VALIDATION_KEYS
    public static string VALIDATION_RESULT = "validationResult";

    #endregion

    public static string quotationMark = "\"";
    public static float REQUEST_TIMEOUT = 20f;

    public const string SERVER_GAME_STATE_URL = "http://sflapi01.schoolfablab.com:9890/API/User/";

    public const string LOGIN_URL_KEY = "Login";
    public const string REGISTER_URL_KEY = "Register";
    public const string FORGOT_PWD_KEY = "Forget?username=";



    public const string FILE_FORMAT = "json";

}

public enum WEB_REQUEST_TYPE
{
    NONE = -1,
    POST = 0,
    GET = 1,
    GRAPH_QUERY,
}


public enum SERVER_UPDATE_REQUEST_TYPE
{
    NONE = -1,
    LOGIN = 1,
    RIGISTERER = 2
}