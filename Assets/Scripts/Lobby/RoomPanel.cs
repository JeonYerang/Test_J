using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    public Text roomTitle;

    public RectTransform playerListTransform;
    public GameObject playerEntryPrefab;

    public Dictionary<int, Transform> playerListDic = new Dictionary<int, Transform>();
    private Dictionary<int, bool> playersReadyDic = new Dictionary<int, bool>();

    public Toggle readyToggle;
    public Button startButton;
    public Button exitButton;

    private void Awake()
    {
        readyToggle.onValueChanged.AddListener(OnReadyToggleChanged);
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnEnable()
    {
        if (false == PhotonNetwork.InRoom) return;

        roomTitle.text = PhotonNetwork.CurrentRoom.Name;

        SetMasterPlayer();
        LoadPlayerList();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        playerListDic.Clear();
    }

    private void SetMasterPlayer()
    {
        readyToggle.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.interactable = false;

        //방장이면 항상 레디상태
        if (PhotonNetwork.IsMasterClient)
            OnReadyToggleChanged(true);
    }

    private void LoadPlayerList()
    {
        foreach(Transform child in playerListTransform)
        {
            Destroy(child.gameObject); //??
        }

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            JoinPlayer(player);

            if (player.CustomProperties.ContainsKey("Ready"))
            {
                SetPlayerReady(player.ActorNumber, (bool)player.CustomProperties["Ready"]);
            }
        }
    }

    public void JoinPlayer(Player newPlayer)
    {
        var playerEntry = Instantiate(playerEntryPrefab, playerListTransform, false);

        if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
        {
            //본인이면?
            playerEntry.transform.Find("NameLabel").GetComponent<Text>().text = $"{newPlayer.NickName} (나)";
        }
        else
        {
            playerEntry.transform.Find("NameLabel").GetComponent<Text>().text = newPlayer.NickName;
        }


        if (newPlayer.IsMasterClient)
        {
            playerEntry.transform.Find("ReadyText").GetComponent<Text>().text = "방장";
        }
        else
        {
            playerEntry.transform.Find("ReadyText").GetComponent<Text>().text = "ready!";
        }


        playerListDic[newPlayer.ActorNumber] = playerEntry.GetComponent<RectTransform>();

        if (PhotonNetwork.IsMasterClient)
        {
            playersReadyDic.Add(newPlayer.ActorNumber, false);
            CheckAllReadys();
        }
    }

    public void LeavePlayer(Player leftPlayer)
    {
        Destroy(playerListDic[leftPlayer.ActorNumber].gameObject);
        playerListDic.Remove(leftPlayer.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            playersReadyDic.Remove(leftPlayer.ActorNumber);
        }
    }

    public void SetPlayerReady(int actorNum, bool isReady)
    {
        GameObject readyText = playerListDic[actorNum].Find("ReadyText").gameObject;
        readyText.SetActive(isReady);

        if (PhotonNetwork.IsMasterClient)
        {
            playersReadyDic[actorNum] = isReady;

            CheckAllReadys();
        }
    }

    private void OnReadyToggleChanged(bool isReady)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        PhotonHashtable customProps = localPlayer.CustomProperties;

        customProps["Ready"] = isReady;

        localPlayer.SetCustomProperties(customProps);
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void CheckAllReadys()
    {
        //모두 레디상태이면 스타트버튼 활성화
        startButton.interactable = playersReadyDic.Values.All(x => x);
    }

    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }
}
