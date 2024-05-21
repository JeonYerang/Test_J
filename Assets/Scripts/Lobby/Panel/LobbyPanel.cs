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

    public Dictionary<string, GameObject> roomDic 
        = new Dictionary<string, GameObject>();
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


    public void UpdateRoomList(List<RoomInfo> roomList) //roomList: ���������� �ִ� ��鸸 ����
    {
        foreach (RoomInfo room in roomList)
        {
            //���� ������ ���
            if (room.RemovedFromList == true)
            {
                roomDic.TryGetValue(room.Name, out GameObject destroyedRoom);
                Destroy(destroyedRoom);
                roomDic.Remove(room.Name);
            }
            else
            {
                //���� ó�� ������ ���
                if (roomDic.ContainsKey(room.Name) == false)
                {
                    GameObject newRoomEntry
                        = Instantiate(roomEntryPrefab, roomListTransform);
                    roomDic.Add(room.Name, newRoomEntry);
                    SetRoomEntry(room, newRoomEntry.transform);
                }
                //������ �����ϴ� ���� ���: �� ���� ������Ʈ
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
        roomEntry.name = roomInfo.Name;

        roomEntry.Find("RoomName").GetComponent<Text>().text = roomInfo.Name;
        roomEntry.Find("MasterName").GetComponent<Text>().text = roomInfo.masterClientId.ToString();
        roomEntry.Find("UserCount").GetComponent<Text>().text 
            = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";

        roomEntry.GetComponent<Button>().onClick.AddListener(
            () => PhotonNetwork.JoinRoom(roomInfo.Name));
    }

    private void OnRandomButtonClick()
    {
        CreateRoomManager.Instance.JoinOrCreateRandomRoom();
    }
}
