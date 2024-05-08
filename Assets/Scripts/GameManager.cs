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

    
}
