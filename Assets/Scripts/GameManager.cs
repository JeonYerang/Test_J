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
        //������ ���Ӿ����� �ٷ� �����Ϸ��� PhotonManager�� �ٷ� �ٿ��� ���
        gameObject.AddComponent<PhotonManager>();

        yield return new WaitUntil(() => isGameReady);

        yield return new WaitForSeconds(1f);

        int playerNum = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        print(playerNum);

        //����� �÷��̾� ���� ����Ʈ�� �÷��̾ �����ϵ���
        var playerPosition = spawnPoints.GetChild(playerNum);

        //Instantiate(Resources.Load("Player")); =>
        //���� ��Ʈ��ũ�� ����ȭ�Ͽ� ����
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
