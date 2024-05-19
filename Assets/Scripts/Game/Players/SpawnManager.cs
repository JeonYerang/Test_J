using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public Transform[] spawnPoints;

    public Dictionary<int, GameObject> spawnedPlayers 
        = new Dictionary<int, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void SpawnCharacter()
    {
        string team = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;
        GameObject spawnedPlayer = null;

        switch (team)
        {
            case "Blue":
                spawnedPlayer
                    = PhotonNetwork.Instantiate("Player", spawnPoints[0].position, Quaternion.identity);
                break;

            case "Red":
                spawnedPlayer
                    = PhotonNetwork.Instantiate("Player", spawnPoints[1].position, Quaternion.identity);
                break;

            default:
                break;
        }

        spawnedPlayers[PhotonNetwork.LocalPlayer.ActorNumber] = spawnedPlayer;
        GameManager.Instance.SetPlayer(spawnedPlayer);
    }

    public void DespawnCharacter()
    {
        int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
        PhotonNetwork.Destroy(spawnedPlayers[actorNum]);
        spawnedPlayers.Remove(actorNum);
        return;
    }
}
