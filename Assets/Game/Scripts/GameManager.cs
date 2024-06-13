using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    PhotonView PV;

    [SerializeField]
    CinemachineVirtualCamera cam;

    public bool isGameStart = false;

    public event EventHandler onGetScore;

    private void Awake()
    {
        Instance = this;
        PV = GetComponent<PhotonView>();
        //DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        if(false == PhotonNetwork.InRoom)
        {
            gameObject.AddComponent<TestManager>();
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

    #region GameReady
    public int startCount;

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

        SpawnManager.Instance.SpawnCharacter();

        isGameStart = true;
    }
    #endregion

    #region Player
    public PlayerMove playerMove { get; private set; }
    public PlayerAttack playerAttack { get; private set; }

    public void SetPlayer(GameObject player)
    {
        playerMove = player.GetComponent<PlayerMove>();
        playerAttack = player.GetComponent<PlayerAttack>();

        cam.Follow = playerMove.transform;
        GamePanelManager.Instance.PanelOpen(GamePanel.Game);
    }
    #endregion

    #region Score
    public int RedTeamScore { get; private set; }
    public int BlueTeamScore {  get; private set; }

    [PunRPC]
    public void GetScore(string teamName, int score)
    {
        if (teamName == "Red")
            RedTeamScore += score;

        else if (teamName == "Blue")
            BlueTeamScore += score;

        onGetScore?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}
