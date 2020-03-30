using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class lobby_controller : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject startbutton, cancelbutton;
    [SerializeField]
    private int roomsize;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        startbutton.SetActive(true);
    }
    public void quickstart()
    {
        startbutton.SetActive(false);
        cancelbutton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Start");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Falha ao entrar na sala:" + message);
        createroom();
    }
    void createroom()
    {
        int roomnumber = Random.Range(0, 10000);
        RoomOptions roomops = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomsize };
        PhotonNetwork.CreateRoom("Room" + roomnumber, roomops);
        Debug.Log(roomnumber);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room");
        createroom();
    }
    public void cancel()
    {
        cancelbutton.SetActive(false);
        startbutton.SetActive(true);
        PhotonNetwork.LeaveRoom();
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
