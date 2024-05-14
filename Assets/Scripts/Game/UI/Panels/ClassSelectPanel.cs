using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ClassSelectPanel : MonoBehaviour
{
    Transform classSelectParent;
    Transform playerListParent;

    private void Awake()
    {
        classSelectParent = transform.Find("ClassSelects");
        playerListParent = transform.Find("PlayerList");

        classToggleGroup = classSelectParent.GetComponent<ToggleGroup>();
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        InitClassList();
    }

    private void OnEnable()
    {
        if (PhotonNetwork.InRoom)
            InitPlayerList();
    }

    #region CountDown
    [SerializeField]
    Text countDownText;
    public void SetCount(int count)
    {
        if (count <= 0)
            countDownText.text = "Start!";
        else
            countDownText.text = $"{count}s...";
    }
    #endregion

    #region ClassList

    [SerializeField]
    GameObject classTogglePrefab;

    ToggleGroup classToggleGroup;
    private List<Toggle> classToggles = new List<Toggle>();

    private void InitClassList()
    {
        int i = 0;
        foreach (PlayerClass playerClass in Enum.GetValues(typeof(PlayerClass)))
        {
            GameObject classEntry = Instantiate(classTogglePrefab, classSelectParent);

            InitClassToggle(classEntry, playerClass, i);

            i++;
        }
    }

    private void InitClassToggle(GameObject classEntry, PlayerClass playerClass, int index)
    {
        //클래스 이름 초기화
        if (classEntry.transform.Find("ClassLabel").TryGetComponent(out TextMeshProUGUI classText))
            classText.text = playerClass.ToString();
        else
            print("클래스 라벨 없음");

        //클래스 이미지 초기화
        if (classEntry.transform.Find("ClassImage").TryGetComponent(out Image classImage))
            classImage.sprite = GameManager.Instance.classList[(int)playerClass].classIcon;
        else
            print("클래스 이미지 없음");

        //클래스 설명 초기화
        if (classEntry.transform.Find("ClassDescription").TryGetComponent(out TextMeshProUGUI classDescription))
            classDescription.text = GameManager.Instance.classList[(int)playerClass].classDescription;
        else
            print("클래스 설명 없음");

        //클래스 토글 초기화
        if (classEntry.transform.TryGetComponent(out Toggle classToggle))
        {
            classToggle = classEntry.transform.GetComponent<Toggle>();

            classToggle.group = classToggleGroup;
            classToggles.Add(classToggle);

            classToggle.onValueChanged.AddListener(
                isOn =>
                {
                    if (isOn)
                    {
                        SelectClass(index);
                    }
                    else
                    {
                        SelectClass(-1);
                    }
                });
        }
        else
            print("클래스 토글 없음");
    }

    public void SelectClass(int select)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable customProps = localPlayer.CustomProperties;

        customProps["Class"] = select;
        localPlayer.SetCustomProperties(customProps);
    }

    public void OnClassPropertyChanged(Player player)
    {
        if (!playerListDic.ContainsKey(player.ActorNumber))
            return;

        Text classLabel =
            playerListDic[player.ActorNumber].transform.Find("SelectLabel").GetComponent<Text>();

        int select = (int)player.CustomProperties["Class"];

        if (select == -1)
            classLabel.text = "선택중...";
        else
            classLabel.text = ((PlayerClass)select).ToString();
    }
    #endregion

    #region PlayerList
    
    [SerializeField]
    GameObject playerEntryPrefab;

    public Dictionary<int, GameObject> playerListDic = new Dictionary<int, GameObject>();
    public void InitPlayerList()
    {
        //기존의 오브젝트 지우기
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }

        playerListDic.Clear();

        //새로운 오브젝트 생성
        //같은 팀의 정보만 띄워준다.
        AddPlayerEntry(PhotonNetwork.LocalPlayer);
        if (PhotonNetwork.LocalPlayer.TryGetTeamMates(out Player[] teamMates))
        {
            foreach (Player teamMate in teamMates)
            {
                AddPlayerEntry(teamMate);
            }
        }
    }

    public void AddPlayerEntry(Player newPlayer)
    {
        GameObject playerEntry = Instantiate(playerEntryPrefab, playerListParent);

        playerEntry.name = newPlayer.ActorNumber.ToString();

        if (playerEntry.transform.Find("NameLabel").TryGetComponent(out Text nameText))
        {
            nameText.text = newPlayer.NickName;

            if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
            {
                nameText.color = Color.green;
            }
        }

        if (playerEntry.transform.Find("SelectLabel").TryGetComponent(out Text classLabel))
        {
            int select = -1;
            if (newPlayer.CustomProperties.ContainsKey("Class"))
            {
                select = (int)newPlayer.CustomProperties["Class"];
            }

            if (select == -1)
            {
                classLabel.text = "선택중...";
            }
            else
            {
                classLabel.text = Enum.GetName(typeof(PlayerClass), (PlayerClass)select);
                classToggles[select].isOn = true;
            }
        }

        playerListDic[newPlayer.ActorNumber] = playerEntry;
    }

    public void RemovePlayerEntry(int leftPlayerActorNum)
    {
        Destroy(playerListDic[leftPlayerActorNum].gameObject);
        playerListDic.Remove(leftPlayerActorNum);
    }
    #endregion
}
