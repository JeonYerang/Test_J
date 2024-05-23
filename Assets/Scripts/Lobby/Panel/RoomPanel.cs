using Photon.Pun;
using Photon.Pun.UtilityScripts;
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

    public Button exitButton;
    public Button teamChangeButton;
    public Toggle readyToggle;
    public Button startButton;

    public RectTransform playerListTransform;
    public GameObject playerEntryPrefab;

    public Dictionary<int, Transform> playerListDic = new Dictionary<int, Transform>();
    private Dictionary<int, bool> playersReadyDic = new Dictionary<int, bool>();

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
        LoadPlayerList();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        ResetPlayerList();
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

        teamChangeButton.gameObject.SetActive(CreateRoomManager.Instance.isTeamMode);

        SetMasterOption();
    }

    private void SetMasterOption()
    {
        //일반 유저는 준비 버튼, 방장은 시작 버튼을 활성화
        readyToggle.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.interactable = false;

        readyToggle.isOn = PhotonNetwork.IsMasterClient; //방장은 항상 레디상태
        //OnReadyToggleChanged(false);
    }

    #region PlayerSet
    private void LoadPlayerList()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            AddPlayerEntry(player);
        }
    }

    private void ResetPlayerList()
    {
        playerListDic.Clear();
        foreach (Transform child in playerListTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddPlayerEntry(Player newPlayer)
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

        //playerList 딕셔너리 세팅
        if (!playerListDic.ContainsKey(newPlayer.ActorNumber))
            playerListDic.Add(newPlayer.ActorNumber, playerEntry.GetComponent<RectTransform>());
        else
            playerListDic[newPlayer.ActorNumber] = playerEntry.GetComponent<RectTransform>();

        //레디 여부
        SetReadyText(newPlayer);

        //팀 색상
        if (CreateRoomManager.Instance.isTeamMode)
            SetTeamColor(newPlayer);
    }

    public void RemovePlayerEntry(Player leftPlayer)
    {
        Destroy(playerListDic[leftPlayer.ActorNumber].gameObject);
        playerListDic.Remove(leftPlayer.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            playersReadyDic.Remove(leftPlayer.ActorNumber);
        }
    }
    #endregion

    #region Ready
    private void OnReadyToggleChanged(bool isReady)
    {
        //print("ReadyToggleChange");
        Player localPlayer = PhotonNetwork.LocalPlayer;
        PhotonHashtable customProps = localPlayer.CustomProperties;

        customProps["Ready"] = isReady;

        localPlayer.SetCustomProperties(customProps);
    }

    public void SetReadyText(Player player)
    {
        if (!player.CustomProperties.ContainsKey("Ready"))
            return;

        int actorNum = player.ActorNumber;
        bool isReady = (bool)player.CustomProperties["Ready"];

        GameObject readyText = playerListDic[actorNum].Find("ReadyText").gameObject;
        readyText.SetActive(isReady);

        if (player.IsLocal && !player.IsMasterClient) //플레이어가 나이고, 방장이 아닐경우
        {
            teamChangeButton.interactable = !isReady; //레디 중에는 팀 변경 안되게
        }

        //ready 딕셔너리 세팅
        if (PhotonNetwork.IsMasterClient)
        {
            if (!playersReadyDic.ContainsKey(actorNum))
                playersReadyDic.Add(actorNum, isReady);
            else
                playersReadyDic[actorNum] = isReady;

            CheckAllReady();
        }
    }

    private void CheckAllReady()
    {
        /*foreach(var actorNum in playersReadyDic.Keys)
        {
            print($"{PhotonNetwork.CurrentRoom.Players[actorNum].NickName} is ready?: {playersReadyDic[actorNum]}");
        }*/
        //모두 레디상태이면 스타트버튼 활성화
        startButton.interactable = playersReadyDic.Values.All(x => x);
    }
    #endregion

    #region Team
    private void OnTeamButtonClick() 
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (localPlayer.GetPhotonTeam().Name == "Blue")
            localPlayer.SwitchTeam("Red");
        else
            localPlayer.SwitchTeam("Blue");
    }
    public void SetTeamColor(Player player)
    {
        if (!player.CustomProperties.ContainsKey("_pt"))
            return;

        Image image = playerListDic[player.ActorNumber].GetComponent<Image>();

        string team = player.GetPhotonTeam().Name;

        switch (team)
        {
            case "Blue":
                image.color = new Color(.7f, .7f, 1);
                break;
            case "Red":
                image.color = new Color(1, .7f, .75f);
                break;
            default:
                image.color = new Color(0.53f, 0.57f, 0.7f);
                break;
        }
    }
    #endregion

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    
}
