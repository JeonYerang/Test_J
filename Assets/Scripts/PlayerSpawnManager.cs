using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

public enum PlayerClass
{
    Warrior,
    Archer,
    Tanker,
    Healer
}

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }

    public GameObject playerPrefab;
    public Dictionary<PlayerClass, ClassData> classDic = new Dictionary<PlayerClass, ClassData>();

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerSpawn()
    {
        GameObject spawnPlayer = Instantiate(playerPrefab);
        //spawnPlayer.SetInfo()
    }
}
