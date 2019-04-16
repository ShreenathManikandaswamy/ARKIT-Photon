using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromServer : Photon.MonoBehaviour
{
    public Transform CubeSP;
    public string AssetName;
    // Start is called before the first frame update
    void Start()
    {
        //string URL = "sftp://hambati@sflapi01.schoolfablab.com:8180/home/users/sfl/assets/Fablab/cube";
        string URL = "http://localhost:8888/Syncor/cube";
        WWW www = new WWW(URL);
        StartCoroutine(WaitForReq(www));
    }

    IEnumerator WaitForReq(WWW www)
    {
        yield return www;
        AssetBundle bundle = www.assetBundle;
        if(www.error == null)
        {
            GameObject Cube = (GameObject)bundle.LoadAsset(AssetName);
            var o = PhotonNetwork.Instantiate(Cube.name, CubeSP.position, CubeSP.rotation, 0);
        }
        else{
            Debug.Log(www.error);
        }
    }
}
