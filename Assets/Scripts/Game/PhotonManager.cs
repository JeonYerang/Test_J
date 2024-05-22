using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }
    GamePanelManager panelManager;

    private void Awake()
    {
        Instance = this;
        panelManager = GamePanelManager.Instance;
    }

    public override void OnLeftRoom()
    {
        SpawnManager.Instance.DespawnCharacter();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        panelManager.selectPanel.AddPlayerEntry(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        panelManager.selectPanel.RemovePlayerEntry(otherPlayer.ActorNumber);
        //panelManager.gamePanel.UserInfo.
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Class"))
        {
            panelManager.selectPanel.OnClassPropertyChanged(targetPlayer);
            panelManager.gamePanel.UserInfo.SetClass(targetPlayer);
        }
    }
}
