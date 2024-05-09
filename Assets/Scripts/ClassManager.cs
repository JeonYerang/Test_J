using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum PlayerClass
{
    Warrior,
    Archer,
    Tanker,
    Healer
}

public class PlayerInfoC
{
    public string playerName;

    public bool team;
    PlayerClass playerClass;
}

public class ClassData : ScriptableObject
{
    public string className;
    public Sprite classImage;
}

public class ClassManager : MonoBehaviourPunCallbacks
{
    public static ClassManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
