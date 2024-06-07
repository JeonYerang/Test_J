using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [HideInInspector]
    public UserInfoArea userInfoArea;

    private void Awake()
    {
        userInfoArea = GetComponent<UserInfoArea>();
        createButton.onClick.AddListener(OnCreateButtonClick);
        randomButton.onClick.AddListener(OnRandomButtonClick);
    }

    private void OnEnable()
    {
        InitCreateArea();

        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        userInfoArea.SetInfo();
    }

    #region Create Room
    [Header("Create Room Menu")]
    public InputField roomNameInput;
    public Dropdown maxCountDrop;
    public Dropdown modeDrop;
    public Button createButton;
    public GameObject warningText;

    private void InitCreateArea()
    {
        roomNameInput.text = $"Room {UnityEngine.Random.Range(0, 99)}";
        maxCountDrop.value = 2;

        modeDrop.ClearOptions();
        List<String> modes = new List<String>();
        foreach (string mode in Enum.GetNames(typeof(GameMode)))
        {
            modes.Add(mode);
        }
        modeDrop.AddOptions(modes);

        warningText.SetActive(false);
    }

    private void OnCreateButtonClick()
    {
        string roomName = roomNameInput.text;

        int maxPlayer = int.Parse(maxCountDrop.options[maxCountDrop.value].text);
        int gameMode = modeDrop.value;

        if (string.IsNullOrEmpty(roomName))
        {
            warningText.SetActive(true);
            return;
        }

        CreateRoomManager.Instance.CreateRoom(roomName, maxPlayer, gameMode);
    }
    #endregion

    [Space(10)]

    #region Find Room
    [Header("Find Room Menu")]
    public RectTransform roomListTransform;
    public GameObject roomEntryPrefab;
    public Button randomButton;

    public Dictionary<string, GameObject> roomEntryDic 
        = new Dictionary<string, GameObject>();
    public List<RoomInfo> currentRoomList = new List<RoomInfo>();

    public void UpdateRoomList(List<RoomInfo> roomList) //roomList: 변동사항이 있는 방들만 전달
    {
        foreach (RoomInfo room in roomList)
        {
            //방이 삭제된 경우
            if (room.RemovedFromList == true)
            {
                if(roomEntryDic.TryGetValue(room.Name, out GameObject destroyedRoom))
                    Destroy(destroyedRoom);
                roomEntryDic.Remove(room.Name);
            }
            else
            {
                //방이 처음 생성된 경우
                if (roomEntryDic.ContainsKey(room.Name) == false)
                {
                    var newRoomEntry
                        = Instantiate(roomEntryPrefab, roomListTransform);
                    roomEntryDic.Add(room.Name, newRoomEntry);
                    SetRoomEntry(room, newRoomEntry.transform);
                }
                //기존에 존재하던 방인 경우: 방 정보 업데이트
                else
                {
                    if (roomEntryDic.TryGetValue(room.Name, out GameObject roomEntry))
                        SetRoomEntry(room, roomEntry.transform);
                }
            }
        }
    }

    private void SetRoomEntry(RoomInfo roomInfo, Transform roomEntry)
    {
        string roomName = roomInfo.Name;
        string gameMode = null;
        string masterName = null;

        if (roomInfo.CustomProperties.ContainsKey("MasterName"))
            masterName = roomInfo.CustomProperties["MasterName"].ToString();

        if (roomInfo.CustomProperties.ContainsKey("GameMode"))
        {
            int modeNum = (int)(roomInfo.CustomProperties["GameMode"]);
            gameMode = Enum.GetName(typeof(GameMode), (GameMode)modeNum);
        }

        roomEntry.name = roomName;


        roomEntry.Find("RoomName").GetComponent<TextMeshProUGUI>().text = roomName;

        if(gameMode != null)
            roomEntry.Find("ModeLabel").GetComponent<TextMeshProUGUI>().text = gameMode;

        if(masterName != null)
            roomEntry.Find("Master").Find("NameLabel").GetComponent<TextMeshProUGUI>().text = masterName;
        
        roomEntry.Find("UserCount").GetComponent<TextMeshProUGUI>().text 
            = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";


        roomEntry.GetComponent<Button>().onClick.AddListener(
            () => PhotonNetwork.JoinRoom(roomName));
    }

    private void OnRandomButtonClick()
    {
        CreateRoomManager.Instance.JoinOrCreateRandomRoom();
    }
    #endregion
}
