using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{

    public InputField userName, password;
    public InputField userName_reg, password_reg, confirmPwd, firName, lastName, SchoolName;//, forName, surName, schoolName, country, state, city;
    public InputField forgotPwdUserNameTxt;

    public GameObject signContent, signUpContent, SuccessPanel,forgetPanel;

    public Text statusText,succesPanelDescrptn;


    public GameObject UnCheckedGO;
    public GameObject CheckedGO;
    public bool keepLoggedIn = false;


    private void Start()
    {
        userName.text = PlayerPrefs.GetString("UserName");
        password.text = PlayerPrefs.GetString("Password");
    }

    public void SignIn()
    {
        Login_API(userName.text, password.text);
        //if (ValidateCredetialsFromLocalDB(userName.text, password.text))
        //{
        //    signContent.SetActive(false);
        //    loginSuccessContent.SetActive(true);
        //    userName.text = string.Empty;
        //    password.text = string.Empty;
        //    Debug.Log("successFully logged in");
        //}
        //else
        //{
        //    sign_status.text = "Wrong Credentials";
        //    Debug.Log("wrong credentials");
        //}
    }
    public void Home()
    {
        closeAllPanels();
        signContent.SetActive(true);

    }

    public void SignUp()
    {
        userName.text = string.Empty;
        password.text = string.Empty;
        closeAllPanels();
        signUpContent.SetActive(true);
    }
    public void OnClickForgotPassword()
    {
        closeAllPanels();
        forgetPanel.SetActive(true);
    }
    public void SubmitForgotPassword()
    {
        ForGotPassword(forgotPwdUserNameTxt.text);
    }
    void closeAllPanels()
    {
        statusText.text = string.Empty;
        signContent.SetActive(false);
        signUpContent.SetActive(false);
        SuccessPanel.SetActive(false);
        forgetPanel.SetActive(false);

    }
    public void Register()
    {
        //UserLocalDB localDB = ServerAPIHandler.Instance.userLocalDB;

        //UserInfo userInfo = new UserInfo();
        //userInfo.userName = userName_reg.text;
        //userInfo.password = password_reg.text;
        //userInfo.firName = firName.text;
        //userInfo.lastName = lastName.text;
        //userInfo.SchoolName = SchoolName.text;

        if (password_reg.text.Equals(confirmPwd.text))
        {
            Regstration_API(userName_reg.text, password_reg.text, firName.text, lastName.text, SchoolName.text, "5", "5", "5");

            //if (localDB.userInfoList == null)
            //    localDB.userInfoList = new List<UserInfo>();

            //bool isUserAlreadyAvailable = false;
            //for (int i = 0; i < localDB.userInfoList.Count; i++)
            //{
            //    if (localDB.userInfoList[i].userName.Equals(userInfo.userName))
            //    {
            //        isUserAlreadyAvailable = true;
            //        continue;
            //    }
            //}

            //if (!isUserAlreadyAvailable)
            //{
            //    localDB.userInfoList.Add(userInfo);

            //    UserLocalDB.SaveToFile(localDB);

            //    signUpContent.SetActive(false);
            //    congratsContent.SetActive(true);

            //    userName_reg.text = string.Empty;
            //     password_reg.text = string.Empty;
            //    confirmPwd.text = string.Empty;
            //   firName.text = string.Empty;
            //   lastName.text = string.Empty; ;
            //     SchoolName.text = string.Empty; 
            //    Debug.Log("registration successfull");
            //}
            //else
            //{
            //    reg_stutus.text = "User Available";
            //    Debug.Log("user already available");
            //}
        }
        else
        {
            statusText.text = "Password does not match";
        }
    }

    public void OnClickOpenSignContent()
    {
        closeAllPanels();
        signContent.SetActive(true);
       
    }

    bool ValidateCredetialsFromLocalDB(string _userName, string _passWord)
    {
        UserLocalDB localDB = ServerAPIHandler.Instance.userLocalDB;
        if (localDB.userInfoList != null)
        {
            for (int i = 0; i < localDB.userInfoList.Count; i++)
            {
                if (localDB.userInfoList[i].userName.Equals(_userName) && localDB.userInfoList[i].password.Equals(_passWord))
                {
                    return true;
                }
            }
        }
        return false;
    }

    #region API CALL

    void Login_API(string _username = null,string _password = null)
    {
        Login login = new Login
        {
            username = _username,
            password = _password,
        };
        string url = ServerConstants.SERVER_GAME_STATE_URL + ServerConstants.LOGIN_URL_KEY;

        ServerAPIHandler.Instance.serverStateManager.UpdateGameState(url, login,(object callBack) =>
        {
            if (callBack != null && callBack.GetType().Equals(typeof(bool)) && (bool)callBack == true)
            {
                Debug.Log("login success");
                statusText.text = string.Empty;
                closeAllPanels();
                SuccessPanel.SetActive(true);
                succesPanelDescrptn.text = "Login successful";
                if(keepLoggedIn){
                    PlayerPrefs.SetString("UserName", userName.text);
                    PlayerPrefs.SetString("Password", password.text);
                }
                else{
                    PlayerPrefs.SetString("CameraName", userName.text);
                }
                userName.text = string.Empty;
                password.text = string.Empty;
                Application.LoadLevel(1);
            }
            else
            {
                WebRequestInfo requestInfo = (WebRequestInfo)callBack;
                statusText.text = requestInfo.errorDescription;
            }
        });
    }

    public void KeepLoogedIn()
    {
        UnCheckedGO.SetActive(false);
        CheckedGO.SetActive(true);
        keepLoggedIn = true;
    }

    public void DontSaveUserInfo()
    {
        UnCheckedGO.SetActive(true);
        CheckedGO.SetActive(false);
        keepLoggedIn = false;
    }

    void ForGotPassword(string _userName)
    {
        ForgorPassword forGotPassWord = new ForgorPassword
        { 
             username = _userName
        };
        string url = ServerConstants.SERVER_GAME_STATE_URL + ServerConstants.FORGOT_PWD_KEY+ _userName;

        ServerAPIHandler.Instance.serverStateManager.UpdateGameState(url, forGotPassWord, (object callBack) =>
        {
            if (callBack != null && callBack.GetType().Equals(typeof(bool)) && (bool)callBack == true)
            {
                closeAllPanels();
                SuccessPanel.SetActive(true);
                succesPanelDescrptn.text = "Please check your mail to reset password";
            }
            else
            {
                WebRequestInfo requestInfo = (WebRequestInfo)callBack;
                statusText.text = requestInfo.errorDescription;
            }
        });
    }
    void Regstration_API(string _username = null,
                         string _password = null,
                         string _forname = null,
                         string _sureName = null,
                         string _schoolName = null,
                         string _country = null,
                         string _state = null,
                         string _city = null)
    {


       Register register = new Register {
            username = _username,
            password = _password,
            forename = _forname,
            surname = _sureName,
            schoolname = _schoolName,
            country = long.Parse(_country),
            state = long.Parse(_state),
            city = long.Parse(_city)

        };

        string url = ServerConstants.SERVER_GAME_STATE_URL + ServerConstants.REGISTER_URL_KEY;

        ServerAPIHandler.Instance.serverStateManager.UpdateGameState(url, register,(object callBack) =>
        {
            if (callBack != null && callBack.GetType().Equals(typeof(bool)) && (bool)callBack== true)
            {
                Debug.Log("registration success");
                statusText.text = string.Empty;
                userName_reg.text = string.Empty;
                     password_reg.text = string.Empty;
                    confirmPwd.text = string.Empty;
                   firName.text = string.Empty;
                   lastName.text = string.Empty; ;
                     SchoolName.text = string.Empty;

                closeAllPanels();
                SuccessPanel.SetActive(true);
                succesPanelDescrptn.text = "Please check your mail to Verify";
            }
            else
            {
                WebRequestInfo requestInfo = (WebRequestInfo)callBack;
                statusText.text = requestInfo.errorDescription;
                Debug.Log("registration failed"); 
            }
        });
    }
    #endregion
}
