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
    private Room room;

    public Text roomTitle;
    public Text modeText;

    public Button exitButton;
    public Button teamChangeButton;
    public Toggle readyToggle;
    public Button startButton;

    public RectTransform playerListTransform;
    public GameObject playerEntryPrefab;

    public Dictionary<int, Transform> playerEntryDic = new Dictionary<int, Transform>();
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
        room = PhotonNetwork.CurrentRoom;

        roomTitle.text = room.Name;

        int mode = -1;
        if (room.CustomProperties.ContainsKey("GameMode"))
        {
            mode = (int)room.CustomProperties["GameMode"];
        }

        if (mode != -1)
        {
            modeText.text = Enum.GetName(typeof(GameMode), (GameMode)mode);
        }

        //if(�����̸�)
        teamChangeButton.gameObject.SetActive(true);

        SetMasterOption();
    }

    private void SetMasterOption()
    {
        //�Ϲ� ������ �غ� ��ư, ������ ���� ��ư�� Ȱ��ȭ
        readyToggle.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
        startButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        startButton.interactable = false;

        //������ �׻� �������
        //readyToggle.isOn = PhotonNetwork.IsMasterClient; 
        readyToggle.SetIsOnWithoutNotify(PhotonNetwork.IsMasterClient);
        OnReadyToggleChanged(PhotonNetwork.IsMasterClient);
    }

    #region PlayerSet
    private void LoadPlayerList()
    {
        foreach (Player player in room.Players.Values)
        {
            AddPlayerEntry(player);
        }
    }

    private void ResetPlayerList()
    {
        playerEntryDic.Clear();
        foreach (Transform child in playerListTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public void AddPlayerEntry(Player newPlayer)
    {
        //������Ʈ ����
        var playerEntry = Instantiate(playerEntryPrefab, playerListTransform, false);

        //�ؽ�Ʈ ����
        Text nameText = playerEntry.transform.Find("NameLabel").GetComponent<Text>();
        nameText.text = newPlayer.NickName;

        if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
        {
            //�����̸�?
            nameText.color = Color.green;
        }

        if (newPlayer.IsMasterClient)
        {
            playerEntry.transform.Find("ReadyText").GetComponent<Text>().text = "����";
        }
        else
        {
            playerEntry.transform.Find("ReadyText").GetComponent<Text>().text = "ready!";
        }

        //playerList ��ųʸ� ����
        if (!playerEntryDic.ContainsKey(newPlayer.ActorNumber))
            playerEntryDic.Add(newPlayer.ActorNumber, playerEntry.GetComponent<RectTransform>());
        else
            playerEntryDic[newPlayer.ActorNumber] = playerEntry.GetComponent<RectTransform>();

        //���� ����
        SetReadyText(newPlayer);

        //�� ����
        //if(�����̸�)
        SetTeamColor(newPlayer);
    }

    public void RemovePlayerEntry(Player leftPlayer)
    {
        Destroy(playerEntryDic[leftPlayer.ActorNumber].gameObject);
        playerEntryDic.Remove(leftPlayer.ActorNumber);
        if (PhotonNetwork.IsMasterClient)
        {
            playersReadyDic.Remove(leftPlayer.ActorNumber);
        }
    }
    #endregion

    #region Ready
    private void OnReadyToggleChanged(bool isReady)
    {
        print(isReady);

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

        GameObject readyText = playerEntryDic[actorNum].Find("ReadyText").gameObject;
        readyText.SetActive(isReady);

        if (player.IsLocal)
        {
            if (!player.IsMasterClient) //������ �ƴҰ��
                teamChangeButton.interactable = !isReady; //���� �߿��� �� ���� �ȵǰ�
            else
                teamChangeButton.interactable = true;
        }

        //ready ��ųʸ� ����
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
        //��� ��������̸� ��ŸƮ��ư Ȱ��ȭ
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

        Image image = playerEntryDic[player.ActorNumber].GetComponent<Image>();

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
        LoadingSceneManager.LoadScene("GameScene");
    }

    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }
    
}