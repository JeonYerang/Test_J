using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }
    public ClassSelectPanel classSelectPanel;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnLeftRoom()
    {
        SpawnManager.instance.DespawnCharacter();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        classSelectPanel.AddPlayerEntry(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        classSelectPanel.RemovePlayerEntry(otherPlayer.ActorNumber);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("Class"))
        {
            classSelectPanel.OnClassPropertyChanged(targetPlayer);
        }
    }
}
