using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.NickName = $"TestPlayer {Random.Range(100, 1000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.IsVisible = false;
        roomOption.MaxPlayers = 8;

        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOption, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        //GameObject.Find("Canvas").transform.Find("DebugText").GetComponent<Text>().text
        //    = PhotonNetwork.CurrentRoom.Name;

        GameManager.isGameReady = true;
    }
}
