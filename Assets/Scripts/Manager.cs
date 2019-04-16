using UnityEngine;
using System.Collections;
using UnityEngine.XR.iOS;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using ExitGames.Client.Photon;
using System.IO;
using System;


public class Manager : Photon.MonoBehaviour
{

    public string verNum = "0.1";
    public string roomName = "room01";
    public string playerName = "player 420";
    public Transform spawnPoint;
    public GameObject GreenAndy;
    public GameObject CameraParent;
    public GameObject ARCameraManager;
    public GameObject LeanSelectPrefab;
    public bool isConnected = false;

    public Transform AndyspawnPoint;

    public GameObject UICamera;
    public GameObject MainUI;
    public GameObject BG;

    public TextMeshProUGUI RealtimeText;
    public GameObject RTT;
    public InputField RTTInputField;
    private string Words = "Hello World";

    /*public string CubeURL;
    public Transform CubeSP;
    public string AssetName;*/


    void Update()
    {
        RealtimeText.text = Words;
        Words = RTTInputField.text;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            PhotonNetwork.offlineMode = true;
        }else{
            PhotonNetwork.offlineMode = false;
        }


    }

    void Start()
    {
        LeanSelectPrefab.SetActive(false);
        MainUI.SetActive(true);
        BG.SetActive(true);
        UICamera.SetActive(true);
        roomName = "Room " + UnityEngine.Random.Range(0, 999);
        playerName = "Player " + UnityEngine.Random.Range(0, 999);
        PhotonNetwork.ConnectUsingSettings(verNum);
        Debug.Log("Starting Connection!");
        RTT.SetActive(false);
    }

    public void OnJoinedLobby()
    {
        //PhotonNetwork.JoinOrCreateRoom (roomName, null, null);
        isConnected = true;
        Debug.Log("Starting Server!");
        BG.SetActive(false);
    }

    public void OnJoinedRoom()
    {
        PhotonNetwork.playerName = playerName;
        isConnected = false;
        spawnPlayer (playerName);
    }


    public void spawnPlayer(string prefName)
    {
        UICamera.SetActive(false);
        LeanSelectPrefab.SetActive(true);
        var ARKitCamera = PhotonNetwork.Instantiate(CameraParent.name, spawnPoint.position, spawnPoint.rotation, 0);
        ARKitCamera.name = PlayerPrefs.GetString("CameraName");
        var CamManager = PhotonNetwork.Instantiate(ARCameraManager.name, spawnPoint.position, spawnPoint.rotation, 0);
        ARKitCamera.GetComponent<CameraParentHelper>().FPSCamera.SetActive(true);
        CamManager.GetComponent<UnityARCameraManager>().enabled = true;
    }

    public void InstantiateObject()
    {
        var o = PhotonNetwork.Instantiate(GreenAndy.name, AndyspawnPoint.position, AndyspawnPoint.rotation, 0);
    }

    /*public void InstantiateAsset()
    {
        WWW www = new WWW(CubeURL);
        //StartCoroutine(DownloadAsset());
        StartCoroutine(WaitForReq(www));
    }

    public IEnumerator WaitForReq(WWW www)
    {
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if (www.error == null)
        {
            GameObject Cube = (GameObject)bundle.LoadAsset(AssetName);
            //var o = PhotonNetwork.Instantiate(Cube.name, CubeSP.position, CubeSP.rotation, 0);
            GameObject player = Instantiate(Cube);
            PhotonView photonView = player.GetComponent<PhotonView>();
            photonView.viewID = 2;
        }
    }*/

    public void TextInstantiate()
    {
        RTT.SetActive(true);
    }

    public void TextInstantiateDone()
    {
        RTT.SetActive(false);
    }

    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        Application.Quit();
    }


    void OnGUI()
    {

        if (isConnected)
        {
            GUI.skin.textField.fontSize = 40; 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 500));
            playerName = GUILayout.TextField(playerName);
            roomName = GUILayout.TextField(roomName);

            if (GUILayout.Button("Create", GUILayout.Width(500), GUILayout.Height(100)))
            {
                PhotonNetwork.JoinOrCreateRoom(roomName, null, null);
            }

            foreach (RoomInfo game in PhotonNetwork.GetRoomList())
            {
                if (GUILayout.Button(game.name + " " + game.playerCount + "/" + game.maxPlayers, GUILayout.Width(500), GUILayout.Height(100)))
                {
                    PhotonNetwork.JoinOrCreateRoom(game.name, null, null);
                }
            }
            GUILayout.EndArea();
        }

    }


}
