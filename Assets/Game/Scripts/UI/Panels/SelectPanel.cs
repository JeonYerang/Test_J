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

public class SelectPanel : Panel
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

    private void OnDisable()
    {
        ResetPlayerList();
    }

    public override void Init()
    {
        panelName = "Select";
    }

    #region CountDown
    [SerializeField]
    TextMeshProUGUI countDownText;
    public void SetCount(int count)
    {
        if (count <= 0)
            countDownText.text = "Start!";
        else
            countDownText.text = $"게임 시작까지 남은 시간: {count}s...";
    }
    #endregion

    #region ClassList

    [SerializeField]
    GameObject classTogglePrefab;

    ToggleGroup classToggleGroup;
    private List<Toggle> classToggles = new List<Toggle>();

    private void InitClassList()
    {
        foreach (PlayerClass playerClass in Enum.GetValues(typeof(PlayerClass)))
        {
            GameObject classEntry = Instantiate(classTogglePrefab, classSelectParent);

            InitClassToggle(classEntry, playerClass);
        }
    }

    private void InitClassToggle(GameObject classEntry, PlayerClass playerClass)
    {
        int index = (int)playerClass;

        //클래스 이름 초기화
        if (classEntry.transform.Find("ClassLabel").TryGetComponent(out TextMeshProUGUI classText))
            classText.text = playerClass.ToString();
        else
            print("클래스 라벨 없음");

        //클래스 이미지 초기화
        if (classEntry.transform.Find("ClassImage").TryGetComponent(out Image classImage))
            classImage.sprite = ClassManager.Instance.classList[index].classIcon;
        else
            print("클래스 이미지 없음");

        //클래스 설명 초기화
        if (classEntry.transform.Find("ClassDescription").TryGetComponent(out TextMeshProUGUI classDescription))
            classDescription.text = ClassManager.Instance.classList[index].classDescription;
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
        Hashtable classProps 
            = new Hashtable(){ { "Class", (int)localPlayer.CustomProperties["Class"] } };

        if((int)classProps["Class"] == select)
        {
            classProps["Class"] = select;
        }
        else
        {
            classProps["Class"] = select;
        }

        localPlayer.SetCustomProperties(classProps);
    }

    public void OnClassPropertyChanged(Player player)
    {
        if (!playerListDic.ContainsKey(player.ActorNumber) 
            || !player.CustomProperties.ContainsKey("Class"))
            return;

        TextMeshProUGUI classLabel =
            playerListDic[player.ActorNumber].transform.Find("SelectLabel").GetComponent<TextMeshProUGUI>();

        int select = (int)player.CustomProperties["Class"];

        if (select == -1)
            classLabel.text = "선택중...";
        else
            classLabel.text = ((PlayerClass)select).ToString();
    }
    #endregion

    #region SelectList
    
    [SerializeField]
    GameObject playerEntryPrefab;

    public Dictionary<int, GameObject> playerListDic = new Dictionary<int, GameObject>();
    private void InitPlayerList()
    {
        //내 정보
        AddPlayerEntry(PhotonNetwork.LocalPlayer);
        //팀원 정보
        if (PhotonNetwork.LocalPlayer.TryGetTeamMates(out Player[] teamMates))
        {
            foreach (Player teamMate in teamMates)
            {
                AddPlayerEntry(teamMate);
            }
        }
    }

    private void ResetPlayerList()
    {
        //기존의 오브젝트 지우기
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }

        playerListDic.Clear();
    }

    public void AddPlayerEntry(Player newPlayer)
    {
        GameObject playerEntry = Instantiate(playerEntryPrefab, playerListParent);

        //이름 세팅
        playerEntry.name = newPlayer.ActorNumber.ToString();

        if (playerEntry.transform.Find("NameLabel").TryGetComponent(out TextMeshProUGUI nameText))
        {
            nameText.text = newPlayer.NickName;

            if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
            {
                nameText.color = Color.green;
            }
        }

        //딕셔너리 추가
        if(playerListDic.ContainsKey(newPlayer.ActorNumber))
            playerListDic[newPlayer.ActorNumber] = playerEntry;

        else
            playerListDic.Add(newPlayer.ActorNumber, playerEntry);


        //직업 세팅
        if (newPlayer.CustomProperties.ContainsKey("Class"))
            OnClassPropertyChanged(newPlayer);

    }

    public void RemovePlayerEntry(int leftPlayerActorNum)
    {
        Destroy(playerListDic[leftPlayerActorNum].gameObject);
        playerListDic.Remove(leftPlayerActorNum);
    }
    #endregion
}
