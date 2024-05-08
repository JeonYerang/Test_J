using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.NickName = $"TestPlayer {Random.Range(100, 1000)}";
        PhotonNetwork.ConnectUsingSettings();
    }

    //룸 커스텀 프로퍼티
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
        //GameObject.Find("Canvas").transform.Find("DebugText").GetComponent<Text>().text
        //    = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
            
        }

        GameManager.isGameReady = true;
    }

    //플레이어 커스텀 프로퍼티 설정
    private void SetPlayerProperties()
    {
        PhotonHashtable playerOption = new PhotonHashtable() {
            { "Team", -1 },
            { "Class", -1 }};

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerOption);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //PhotonNetwork.PlayerList[i].SetCustomProperties(
            //    new PhotonHashtable { { "IsAdmin", "Admin" } });
        }
    }
}
