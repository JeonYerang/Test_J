using Photon.Pun;
using Photon.Realtime;
using System;
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
    public Text modeText;

    public RectTransform playerListTransform;
    public GameObject playerEntryPrefab;

    public Dictionary<int, Transform> playerListDic = new Dictionary<int, Transform>();
    private Dictionary<int, bool> playersReadyDic = new Dictionary<int, bool>();

    public Button exitButton;
    public Button teamChangeButton;
    public Toggle readyToggle;
    public Button startButton;

    private void Awake()
    {
        teamChangeButton.onClick.AddListener(OnTeamButtonClick);
        readyToggle.onValueChanged.AddListener(OnReadyToggleChanged);
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnEnable()
    {
        if (false == PhotonNetwork.InRoom) return;

        InitPanel();

        SetMasterPlayer();
        LoadPlayerList();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        playerListDic.Clear();
    }

    private void InitPanel()
    {
        roomTitle.text = PhotonNetwork.CurrentRoom.Name;

        int mode = -1;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("GameMode"))
        {
            mode = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"];
        }

        if (mode != -1)
        {
            modeText.text = Enum.GetName(typeof(GameMode), (GameMode)mode);
        }

        teamChangeButton.gameObject.SetActive(RoomManager.Instance.isTeamMode);
    }

    #region PlayerSet
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
        foreach (Transform child in playerListTransform)
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

            if (player.CustomProperties.ContainsKey("Team"))
            {
                SetPlayerTeam(player.ActorNumber, (int)player.CustomProperties["Team"]);
            }
        }
    }

    public void JoinPlayer(Player newPlayer)
    {
        //오브젝트 생성
        var playerEntry = Instantiate(playerEntryPrefab, playerListTransform, false);

        //텍스트 설정
        Text nameText = playerEntry.transform.Find("NameLabel").GetComponent<Text>();
        nameText.text = newPlayer.NickName;

        if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
        {
            //본인이면?
            nameText.color = Color.green;
        }

        if (newPlayer.IsMasterClient)
        {
            playerEntry.transform.Find("ReadyText").GetComponent<Text>().text = "방장";
        }
        else
        {
            playerEntry.transform.Find("ReadyText").GetComponent<Text>().text = "ready!";
        }

        //딕셔너리 추가
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

    public void SetPlayerTeam(int actorNum, int team)
    {
        Image image = playerListDic[actorNum].GetComponent<Image>();
        if (team == 0)
            image.color = new Color(.7f, .7f, 1);
        else if (team == 1)
            image.color = new Color(1, .7f, .75f);
    }
    #endregion
    private void CheckAllReadys()
    {
        //모두 레디상태이면 스타트버튼 활성화
        startButton.interactable = playersReadyDic.Values.All(x => x);
    }

    #region OnClickButtons
    private void OnReadyToggleChanged(bool isReady)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        PhotonHashtable customProps = localPlayer.CustomProperties;

        customProps["Ready"] = isReady;

        localPlayer.SetCustomProperties(customProps);
    }

    private void OnTeamButtonClick()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        PhotonHashtable customProps = localPlayer.CustomProperties;

        customProps["Team"] = (int)customProps["Team"] == 0 ? 1 : 0;

        localPlayer.SetCustomProperties(customProps);
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}
