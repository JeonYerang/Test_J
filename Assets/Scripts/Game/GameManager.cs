using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public enum GameMode
{
    Occupation
}

public enum PlayerClass
{
    Warrior,
    Archer,
    Tanker,
    Healer
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    PhotonView PV;

    public int startCount;
    public bool isGameStart = false;
    public static bool isGameReady = false;

    public ClassData[] classList = new ClassData[3];

    private void Awake()
    {
        Instance = this;
        PV = GetComponent<PhotonView>();
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (false == PhotonNetwork.InRoom)
        {
            gameObject.AddComponent<TestManager>();
        }
    }

    private void OnEnable()
    {
        if(false == PhotonNetwork.InRoom)
        {
            StartCoroutine(DebugModeSetting());
        }
        else
        {
            ReadyForGame();
        }
    }

    IEnumerator DebugModeSetting()
    {
        yield return new WaitUntil(
            () => PhotonNetwork.InRoom);

        ReadyForGame();
    }

    public void ReadyForGame()
    {
        SetClassProperties(PhotonNetwork.LocalPlayer);

        GamePanelManager.Instance.PanelOpen(GamePanel.Select);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(StartCountDown());
        }
    }

    private void SetClassProperties(Player player)
    {
        PhotonHashtable playerOption = player.CustomProperties;

        playerOption["Class"] = -1;

        player.SetCustomProperties(playerOption);
    }

    private IEnumerator StartCountDown()
    {
        int currentCount = startCount;

        while(currentCount >= 0)
        {
            PV.RPC("ShowTimer", RpcTarget.All, currentCount);
            yield return new WaitForSeconds(1f);
            currentCount--;
        }

        PV.RPC("GameStart", RpcTarget.All);
    }

    [PunRPC]
    void ShowTimer(int sec)
    {
        GamePanelManager.Instance.selectPanel.SetCount(sec);
    }

    [PunRPC]
    void GameStart()
    {
        StartCoroutine(CheckForGameStart());
    }

    IEnumerator CheckForGameStart()
    {
        yield return new WaitUntil(
            () => (int)PhotonNetwork.LocalPlayer.CustomProperties["Class"] != -1);

        yield return new WaitForSeconds(1f);

        GamePanelManager.Instance.PanelOpen(GamePanel.Game);
        SpawnManager.instance.SpawnCharacter();
        isGameStart = true;
    }
}
