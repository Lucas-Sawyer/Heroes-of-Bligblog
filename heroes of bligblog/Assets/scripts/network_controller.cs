using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class network_controller : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("conectado:" + PhotonNetwork.CloudRegion);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
