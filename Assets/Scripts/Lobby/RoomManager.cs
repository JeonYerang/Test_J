using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance {  get; private set; }

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
        /*PhotonNetwork.LocalPlayer.JoinTeam("Red");
        //print($"MyTeam: {PhotonNetwork.LocalPlayer.GetPhotonTeam().Name}");
        if(PhotonTeamsManager.Instance.TryGetTeamMembers("Red", out Player[] members))
        {
            if (members.Length <= 0)
                print("Null");

            foreach (var member in members)
                print(member.NickName);

        }
        
        int i = PhotonTeamsManager.Instance.GetTeamMembersCount("Red");
        print($"TeamCount: {i}");*/


        Hashtable playerOption = new Hashtable();

        if (!isTeamMode)
        {
            playerOption.Add("Team", -1);
        }
        else
        {
            int a = 0, b = 0, c = 0;
            foreach(var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                if (player.CustomProperties.ContainsKey("Team"))
                {
                    if ((int)player.CustomProperties["Team"] == 0)
                        a++;
                    else
                        b++;
                }
            }

            c = a > b ? 1 : 0;
            playerOption.Add("Team", c);

            /*switch (c)
            {
                case 0:
                    print("Blue");
                    break;
                case 1:
                    print("Red");
                    break;
                default:
                    print("None");
                    break;
            }*/
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerOption);
    }
}
