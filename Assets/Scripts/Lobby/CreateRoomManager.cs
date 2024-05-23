using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomManager : MonoBehaviour
{
    public static CreateRoomManager Instance {  get; private set; }

    public string roomName;
    public int maxPlayer;
    public int gameMode;

    public bool isTeamMode { get { return gameMode == 0; } }

    private void Awake()
    {
        Instance = this;
    }

    public void CreateRoom(string roomName, int maxPlayer, int gameMode)
    {
        this.roomName = roomName;
        this.maxPlayer = maxPlayer;
        this.gameMode = gameMode;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.CustomRoomProperties = new Hashtable() {
            { "GameMode", gameMode }};

        PhotonNetwork.CreateRoom(roomName, options);
    }

    public void JoinOrCreateRandomRoom()
    {
        string roomName = $"Room {Random.Range(0, 99)}";

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        int randMode = Random.Range(0, System.Enum.GetValues(typeof(GameMode)).Length);
        options.CustomRoomProperties = new Hashtable() {
            { "GameMode", randMode }};

        this.roomName = roomName;
        this.maxPlayer = 8;
        this.gameMode = randMode;

        PhotonNetwork.JoinRandomOrCreateRoom(roomName: roomName, roomOptions: options);
    }

    public void InitTeam()
    {
        Player player = PhotonNetwork.LocalPlayer;

        int BlueTeamCount = PhotonTeamsManager.Instance.GetTeamMembersCount("Blue");
        int RedTeamCount = PhotonTeamsManager.Instance.GetTeamMembersCount("Red");

        string playerTeam = BlueTeamCount > RedTeamCount ? "Red" : "Blue";
        //print($"Blue: {BlueTeamCount}, Red: {RedTeamCount}");

        if(player.GetPhotonTeam() != null)
        {
            print($"SwitchTeam: {player.GetPhotonTeam().Name}");
            print($"To: {playerTeam}");
            player.SwitchTeam(playerTeam);
            print($"Res: {player.GetPhotonTeam().Name}");
        }
        else
        {
            player.JoinTeam(playerTeam);
        }
        //print($"Blue: {BlueTeamCount}, Red: {RedTeamCount}");
    }
}
