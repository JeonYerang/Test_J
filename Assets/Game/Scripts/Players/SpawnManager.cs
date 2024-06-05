using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public Transform[] spawnPoints;

    public Dictionary<int, GameObject> spawnedPlayers 
        = new Dictionary<int, GameObject>();

    private void Awake()
    {
        Instance = this;
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

    private void SetPlayerClassComponent(Player player)
    {
        switch ((PlayerClass)player.CustomProperties["Class"])
        {
            case PlayerClass.Tanker:
                
                break;

            case PlayerClass.Warrior:

                break;

            case PlayerClass.Archer:

                break;

            case PlayerClass.Healer:

                break;
        }
    }

    public void DespawnCharacter()
    {
        int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
        PhotonNetwork.Destroy(spawnedPlayers[actorNum]);
        spawnedPlayers.Remove(actorNum);
        return;
    }
}
