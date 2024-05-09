using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Transform spawnPoints;

    public static bool isGameReady = false;

    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this);
    }

    private void Start()
    {
        SetPlayerProperties(PhotonNetwork.LocalPlayer);

        if (false == PhotonNetwork.InRoom)
        {
            //StartCoroutine(DebugStart());
        }
        else
        {
            //normal game start
        }
    }

    /*private IEnumerator StartCountDown(int sec)
    {

    }*/

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

    private void SetPlayerProperties(Player player)
    {
        PhotonHashtable playerOption = new PhotonHashtable() {
            { "Team", -1 },
            { "Class", -1 }};

        player.SetCustomProperties(playerOption);

        /*for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //PhotonNetwork.PlayerList[i].SetCustomProperties(
            //    new PhotonHashtable { { "IsAdmin", "Admin" } });
        }*/
    }
}
