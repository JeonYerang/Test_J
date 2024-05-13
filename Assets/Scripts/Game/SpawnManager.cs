using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public Transform[] spawnPoints;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnCharacter()
    {
        GameObject spawnedPlayer
            = PhotonNetwork.Instantiate("Player", spawnPoints[0].position, Quaternion.identity);

        //PlayerInfo playerInfo = spawnedPlayer.GetComponent<PlayerInfo>();
    }
}
