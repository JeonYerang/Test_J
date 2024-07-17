using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }
    PanelManager panelManager;

    private void Awake()
    {
        Instance = this;
        panelManager = PanelManager.Instance;
    }

    public override void OnLeftRoom()
    {
        SpawnManager.Instance.DespawnCharacter();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        (panelManager.GetPanel(PanelName.Select) as SelectPanel)?.AddPlayerEntry(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        (panelManager.GetPanel(PanelName.Select) as SelectPanel)?.RemovePlayerEntry(otherPlayer.ActorNumber);
        //panelManager.gamePanel.UserInfo.
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Class"))
        {
            if (panelManager != null)
            {
                (panelManager.GetPanel(PanelName.Select) as SelectPanel)?.OnClassPropertyChanged(targetPlayer);
                GameUIManager.Instance.UserInfo.SetClass(targetPlayer);
            }
        }
    }
}
