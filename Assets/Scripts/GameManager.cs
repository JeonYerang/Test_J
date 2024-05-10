using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public enum GameMode
{
    Occupation
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PhotonView PV;

    public Transform spawnPoints;

    public int countDown;
    public static bool isGameReady = false;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SetClassProperties(PhotonNetwork.LocalPlayer);

        if (PhotonNetwork.IsMasterClient)
        {
            StartCountDown(10);
        }

        if (false == PhotonNetwork.InRoom)
        {
            //StartCoroutine(DebugStart());
        }
        else
        {
            //normal game start
        }
    }

    private void OnEnable()
    {
        GameUIManager.Instance.ShowSelectPanel();
    }

    private IEnumerator StartCountDown(int sec)
    {
        countDown = sec;

        while(countDown > 0)
        {
            yield return new WaitForSeconds(1f);
            countDown--;
            PV.RPC("ShowTimer", RpcTarget.All, countDown);
        }
    }

    [PunRPC]
    void ShowTimer(int sec)
    {
        GameUIManager.Instance.classSelectPanel.SetCount(countDown);
    }

    private IEnumerator DebugStart()
    {
        //디버깅시 게임씬에서 바로 시작하려면 PhotonManager를 바로 붙여서 사용
        gameObject.AddComponent<PhotonManager>();

        yield return new WaitUntil(() => isGameReady);

        yield return new WaitForSeconds(1f);

        int playerNum = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        print(playerNum);

        //저장된 플레이어 생성 포인트에 플레이어를 생성하도록
        var playerPosition = spawnPoints.GetChild(playerNum);

        //Instantiate(Resources.Load("Player")); =>
        //포톤 네트워크에 동기화하여 생성
        var playerObject =
            PhotonNetwork.Instantiate("Player", playerPosition.position, playerPosition.rotation);
        playerObject.name = $"Player {playerNum}";

        if (false == PhotonNetwork.IsMasterClient)
        {
            yield break;
        }
    }

    private void SetClassProperties(Player player)
    {
        PhotonHashtable playerOption = player.CustomProperties;

        playerOption.Add("Class", -1);

        player.SetCustomProperties(playerOption);
    }
}
