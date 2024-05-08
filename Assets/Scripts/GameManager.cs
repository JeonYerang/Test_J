using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (false == PhotonNetwork.InRoom)
        {
            StartCoroutine(DebugStart());
            return;
        }
        else
        {
            //normal game start
        }
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

    
}
