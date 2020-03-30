using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class room_controller : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int index_multplayer;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("joined");
        StartGame();
    }
    private void StartGame()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Game");
            PhotonNetwork.LoadLevel(index_multplayer);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
