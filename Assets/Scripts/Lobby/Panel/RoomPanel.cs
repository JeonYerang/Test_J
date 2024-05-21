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

        SetMasterOption();
        LoadPlayerList();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDisable()
    {
        ResetPlayerList();
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
    private void SetMasterOption()
    {
        //일반 유저는 준비 버튼, 방장은 시작 버튼을 활성화
        readyToggle.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.interactable = false;

        readyToggle.isOn = false;
    }

    private void LoadPlayerList()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            JoinPlayer(player);
        }
    }

    private void ResetPlayerList()
    {
        foreach (Transform child in playerListTransform)
        {
            Destroy(child.gameObject);
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

        //playerList 딕셔너리 추가
        playerListDic[newPlayer.ActorNumber] = playerEntry.GetComponent<RectTransform>();

        //팀 색상 지정
        if(RoomManager.Instance.isTeamMode)
            SetEntryTeamColor(newPlayer);

        //ready 딕셔너리 추가
        SetPlayerReady(newPlayer);
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

    public void SetPlayerReady(Player player)
    {
        if (!player.CustomProperties.ContainsKey("Ready"))
            return;

        int actorNum = player.ActorNumber;
        bool isReady = (bool)player.CustomProperties["Ready"];

        GameObject readyText = playerListDic[actorNum].Find("ReadyText").gameObject;
        readyText.SetActive(isReady);
        teamChangeButton.interactable = !isReady;

        print(PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient)
        {
            print(player.IsLocal);

            if (!player.IsLocal)
            {
                if (!playersReadyDic.ContainsKey(actorNum))
                    playersReadyDic.Add(actorNum, isReady);
                else
                    playersReadyDic[actorNum] = isReady;
            }

            CheckAllReadys();
        }
    }

    public void SetEntryTeamColor(Player player)
    {
        if(!player.CustomProperties.ContainsKey("_pt"))
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

    private void OnTeamButtonClick() //레디 중에는 팀 변경 안되게
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;

        if (localPlayer.GetPhotonTeam().Name == "Blue")
            localPlayer.SwitchTeam("Red");
        else
            localPlayer.SwitchTeam("Blue");
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
