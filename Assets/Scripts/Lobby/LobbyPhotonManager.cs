using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPhotonManager : MonoBehaviourPunCallbacks
{
    public static LobbyPhotonManager Instance;
    private PanelManager panelManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        panelManager = PanelManager.Instance;
    }

    public override void OnConnected()
    {
        StartCoroutine(WaitConnected());
    }

    IEnumerator WaitConnected()
    {
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        PhotonNetwork.JoinLobby();
        panelManager.PanelOpen("Lobby");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print($"disconnected cause: {cause}");
        panelManager.PanelOpen("Login");
    }

    public override void OnJoinedLobby()
    {
        panelManager.PanelOpen("Lobby");
    }

    public override void OnLeftLobby()
    {
        panelManager.PanelOpen("Login");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        panelManager.lobbyPanel.UpdateRoomList(roomList);
    }

    public override void OnCreatedRoom()
    {
        panelManager.PanelOpen("Room");
    }

    public override void OnJoinedRoom()
    {
        RoomManager.Instance.InitTeam();

        panelManager.PanelOpen("Room");
    }

    public override void OnLeftRoom()
    {
        panelManager.PanelOpen("Lobby");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        panelManager.roomPanel.JoinPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        panelManager.roomPanel.LeavePlayer(otherPlayer);
    }

    //커스텀 프로퍼티가 변경되면 호출되는 함수
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
        {
            panelManager.roomPanel.SetPlayerReady(targetPlayer.ActorNumber, (bool)changedProps["Ready"]);
        }
        if (changedProps.ContainsKey("Team"))
        {
            panelManager.roomPanel.SetPlayerTeam(targetPlayer.ActorNumber, (int)changedProps["Team"]);
        }
    }
}
