using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public enum PlayerClass
{
    Warrior,
    Archer,
    Tanker,
    Healer
}

public enum PlayerTeam
{
    Red,
    Blue
}

public class PlayerInfoC
{
    public string playerName;

    public bool team;
    PlayerClass playerClass;
}

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance { get; private set; }

    public Dictionary<PlayerClass, GameObject> classDic = new Dictionary<PlayerClass, GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    public void DecisionClass()
    {
        
    }
}
