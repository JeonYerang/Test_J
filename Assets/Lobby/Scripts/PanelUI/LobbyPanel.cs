using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour
{
    [HideInInspector]
    public UserInfoArea userInfoArea;

    #region Create Room
    [Header("Create Room Menu")]
    public InputField roomNameInput;
    public Dropdown maxCountDrop;
    public Dropdown modeDrop;
    public Button createButton;
    public GameObject warningText;
    #endregion

    [Space(10)]

    #region Find Room
    [Header("Find Room Menu")]
    public RectTransform roomListTransform;
    public GameObject roomEntryPrefab;
    public Button randomButton;

    public Dictionary<string, Transform> roomEntryDic 
        = new Dictionary<string, Transform>();
    public List<RoomInfo> currentRoomList = new List<RoomInfo>();
    #endregion

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

    #region CreateRoom
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

    #region FindRoom
    public void UpdateRoomList(List<RoomInfo> roomList) //roomList: 변동사항이 있는 방들만 전달
    {
        foreach (RoomInfo room in roomList)
        {
            //방이 삭제된 경우
            if (room.RemovedFromList == true)
            {
                roomEntryDic.TryGetValue(room.Name, out Transform destroyedRoom);
                Destroy(destroyedRoom);
                roomEntryDic.Remove(room.Name);
            }
            else
            {
                //방이 처음 생성된 경우
                if (roomEntryDic.ContainsKey(room.Name) == false)
                {
                    Transform newRoomEntry
                        = Instantiate(roomEntryPrefab, roomListTransform).transform;
                    roomEntryDic.Add(room.Name, newRoomEntry);
                    SetRoomEntry(room, newRoomEntry.transform);
                }
                //기존에 존재하던 방인 경우: 방 정보 업데이트
                else
                {
                    var newRoomEntry = roomListTransform.Find(room.Name).transform;
                    SetRoomEntry(room, newRoomEntry);
                }
            }
        }
    }

    private void SetRoomEntry(RoomInfo roomInfo, Transform roomEntry)
    {
        string roomName = roomInfo.Name;

        if (roomInfo.CustomProperties.ContainsKey("Master"))


        roomEntry.name = roomName;

        roomEntry.Find("RoomName").GetComponent<Text>().text = roomName;
        roomEntry.Find("MasterName").GetComponent<Text>().text = roomInfo.CustomProperties["Master"].ToString();
        roomEntry.Find("UserCount").GetComponent<Text>().text 
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
