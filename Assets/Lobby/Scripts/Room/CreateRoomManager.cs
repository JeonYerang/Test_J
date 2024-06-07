using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum GameMode
{
    Domination
}

public class CreateRoomManager : MonoBehaviour
{
    public static CreateRoomManager Instance {  get; private set; }

    public List<Room> roomList = new List<Room>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreateRoom(string roomName, int maxPlayer, int gameMode)
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.CustomRoomProperties = new Hashtable() {
            { "MasterName", PhotonNetwork.LocalPlayer.NickName },
            { "GameMode", gameMode }};

        options.CustomRoomPropertiesForLobby 
            = new string[] { "MasterName", "GameMode" };

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinOrCreateRandomRoom()
    {
        string roomName = $"Room {Random.Range(0, 99)}";

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        int randMode = Random.Range(0, System.Enum.GetValues(typeof(GameMode)).Length);
        options.CustomRoomProperties = new Hashtable() {
            { "MasterName", PhotonNetwork.LocalPlayer.NickName },
            { "GameMode", randMode }};

        options.CustomRoomPropertiesForLobby
            = new string[] { "MasterName", "GameMode" };

        PhotonNetwork.JoinRandomOrCreateRoom(roomName: roomName, roomOptions: options);
    }

    public void InitTeam() //Check: 한 번 입장했었던 방에 재입장하는 경우 확인 필요
    {
        Player player = PhotonNetwork.LocalPlayer;

        int BlueTeamCount = PhotonTeamsManager.Instance.GetTeamMembersCount("Blue");
        int RedTeamCount = PhotonTeamsManager.Instance.GetTeamMembersCount("Red");

        string playerTeam = BlueTeamCount > RedTeamCount ? "Red" : "Blue";

        if(player.GetPhotonTeam() != null)
        {
            player.SwitchTeam(playerTeam);
        }
        else
        {
            player.JoinTeam(playerTeam);
        }
    }
}
