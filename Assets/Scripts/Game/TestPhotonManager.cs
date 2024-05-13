using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TestPhotonManager : MonoBehaviourPunCallbacks
{
    public static TestPhotonManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.NickName = $"TestPlayer {Random.Range(100, 1000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 8;
        roomOption.CustomRoomProperties = new Hashtable() {
                { "GameMode", 0 }};

        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOption, null);
    }

    public override void OnJoinedRoom()
    {
        print($"{PhotonNetwork.CurrentRoom.Name}에 참가함");
        Hashtable playerOption = new Hashtable();
        playerOption.Add("Team", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerOption);
    }
}
