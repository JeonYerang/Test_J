using Photon.Pun;
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
    [SerializeField]
    Text countDownText;

    Transform classSelectParent;
    [SerializeField]
    GameObject classSelectPrefab;

    Transform playerListParent;
    [SerializeField]
    GameObject playerEntryPrefab;

    ToggleGroup classToggleGroup;
    private List<Toggle> classToggles = new List<Toggle>();

    public Dictionary<int, GameObject> playerListDic = new Dictionary<int, GameObject>();

    private void Awake()
    {
        classSelectParent = transform.Find("ClassSelects");
        playerListParent = transform.Find("PlayerList");

        classToggleGroup = classSelectParent.GetComponent<ToggleGroup>();

        InitSelectList();
    }

    private void OnEnable()
    {
        if (PhotonNetwork.InRoom)
            InitPlayerList();
    }

    public void SetCount(int count)
    {
        if (count <= 0)
            countDownText.text = "Start!";
        else
            countDownText.text = $"{count}s...";
    }

    public void InitPlayerList()
    {
        //기존의 오브젝트 지우기
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }

        playerListDic.Clear();

        //새로운 오브젝트 생성
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            //같은 팀의 정보만 띄워준다.
            if (player.CustomProperties["Team"] 
                == PhotonNetwork.LocalPlayer.CustomProperties["Team"])
                AddPlayerEntry(player);
        }
    }

    private void InitSelectList()
    {
        int i = 0;
        foreach (PlayerClass playerClass in Enum.GetValues(typeof(PlayerClass)))
        {
            GameObject classEntry = Instantiate(classSelectPrefab, classSelectParent);

            if (classEntry.transform.Find("ClassLabel").TryGetComponent(out TextMeshProUGUI classText))
                classText.text = playerClass.ToString();

            Toggle classToggle = classEntry.transform.GetComponent<Toggle>();

            classToggle.group = classToggleGroup;
            classToggles.Add(classToggle);

            int index = i;
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

            i++;
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

    public void RemovePlayerEntry(Player leftPlayer)
    {
        Destroy(playerListDic[leftPlayer.ActorNumber].gameObject);
        playerListDic.Remove(leftPlayer.ActorNumber);
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
        Text classLabel = 
            playerListDic[player.ActorNumber].transform.Find("SelectLabel").GetComponent<Text>();

        int select = (int)player.CustomProperties["Class"];

        if (select == -1)
            classLabel.text = "선택중...";
        else
            classLabel.text = ((PlayerClass)select).ToString();
    }
}
