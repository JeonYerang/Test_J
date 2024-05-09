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
        //roomOption.CustomRoomProperties = new PhotonHashtable() { 
        //    { "GameMode", -1 }};
        
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

    //플레이어 커스텀 프로퍼티 설정
    private void SetPlayerProperties(Player player)
    {
        PhotonHashtable playerOption = new PhotonHashtable() {
            { "Team", -1 },
            { "Class", -1 }};

        player.SetCustomProperties(playerOption);

        /*for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //PhotonNetwork.PlayerList[i].SetCustomProperties(
            //    new PhotonHashtable { { "IsAdmin", "Admin" } });
        }*/
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if(changedProps.ContainsKey("Class"))
        {
            classSelectPanel.OnClassPropertyChanged(targetPlayer);
        }
    }
}
