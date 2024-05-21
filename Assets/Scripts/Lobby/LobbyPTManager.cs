using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class LobbyPTManager : MonoBehaviourPunCallbacks
{
    public static LobbyPTManager Instance;
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
        if (RoomManager.Instance.isTeamMode) //������ ���
            RoomManager.Instance.InitTeam();

        panelManager.PanelOpen("Room");
    }

    public override void OnLeftRoom()
    {
        panelManager.PanelOpen("Lobby");

        Player player = PhotonNetwork.LocalPlayer;

        //�� ���� �ʱ�ȭ
        if (player.GetPhotonTeam() != null)
            player.LeaveCurrentTeam();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        panelManager.roomPanel.JoinPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        panelManager.roomPanel.LeavePlayer(otherPlayer);
    }

    //Ŀ���� ������Ƽ�� ����Ǹ� ȣ��Ǵ� �Լ�
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Ready"))
        {
            panelManager.roomPanel.SetPlayerReady(targetPlayer);
        }
        if (changedProps.ContainsKey("_pt"))
        {
            panelManager.roomPanel.SetEntryTeamColor(targetPlayer);
        }
    }
}
