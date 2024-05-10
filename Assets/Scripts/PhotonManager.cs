using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }
    public ClassSelectPanel classSelectPanel;
    public GameUIManager uiManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (false == PhotonNetwork.InRoom)
        {
            PhotonNetwork.NickName = $"TestPlayer {Random.Range(100, 1000)}";
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {

        }
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 8;
        roomOption.CustomRoomProperties = new PhotonHashtable() {
                { "GameMode", 0 }};

        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOption, null);
    }

    public override void OnJoinedRoom()
    {
        print($"{PhotonNetwork.CurrentRoom.Name}에 참가함");

        //public bool IsMine { get => player == PhotonNetwork.LocalPlayer; }
        //public Player player;

        
        /*if (PhotonNetwork.IsMasterClient)
        {
            
        }*/

        //GameManager.isGameReady = true;

        classSelectPanel.InitPlayerList();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        classSelectPanel.AddPlayerEntry(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        classSelectPanel.RemovePlayerEntry(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if(changedProps.ContainsKey("Class"))
        {
            classSelectPanel.OnClassPropertyChanged(targetPlayer);
        }
    }
}
