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

public class SelectPanel : MonoBehaviour
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

    #region CountDown
    [SerializeField]
    TextMeshProUGUI countDownText;
    public void SetCount(int count)
    {
        if (count <= 0)
            countDownText.text = "Start!";
        else
            countDownText.text = $"���� ���۱��� ���� �ð�: {count}s...";
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

        //Ŭ���� �̸� �ʱ�ȭ
        if (classEntry.transform.Find("ClassLabel").TryGetComponent(out TextMeshProUGUI classText))
            classText.text = playerClass.ToString();
        else
            print("Ŭ���� �� ����");

        //Ŭ���� �̹��� �ʱ�ȭ
        if (classEntry.transform.Find("ClassImage").TryGetComponent(out Image classImage))
            classImage.sprite = ClassManager.Instance.classList[index].classIcon;
        else
            print("Ŭ���� �̹��� ����");

        //Ŭ���� ���� �ʱ�ȭ
        if (classEntry.transform.Find("ClassDescription").TryGetComponent(out TextMeshProUGUI classDescription))
            classDescription.text = ClassManager.Instance.classList[index].classDescription;
        else
            print("Ŭ���� ���� ����");

        //Ŭ���� ��� �ʱ�ȭ
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
            print("Ŭ���� ��� ����");
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
        if (!playerListDic.ContainsKey(player.ActorNumber) 
            || !player.CustomProperties.ContainsKey("Class"))
            return;

        TextMeshProUGUI classLabel =
            playerListDic[player.ActorNumber].transform.Find("SelectLabel").GetComponent<TextMeshProUGUI>();

        int select = (int)player.CustomProperties["Class"];

        if (select == -1)
            classLabel.text = "������...";
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
        //�� ����
        AddPlayerEntry(PhotonNetwork.LocalPlayer);
        //���� ����
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
        //������ ������Ʈ �����
        foreach (Transform child in playerListParent)
        {
            Destroy(child.gameObject);
        }

        playerListDic.Clear();
    }

    public void AddPlayerEntry(Player newPlayer)
    {
        GameObject playerEntry = Instantiate(playerEntryPrefab, playerListParent);

        //�̸� ����
        playerEntry.name = newPlayer.ActorNumber.ToString();

        if (playerEntry.transform.Find("NameLabel").TryGetComponent(out TextMeshProUGUI nameText))
        {
            nameText.text = newPlayer.NickName;

            if (PhotonNetwork.LocalPlayer.ActorNumber == newPlayer.ActorNumber)
            {
                nameText.color = Color.green;
            }
        }

        //��ųʸ� �߰�
        if(playerListDic.ContainsKey(newPlayer.ActorNumber))
            playerListDic[newPlayer.ActorNumber] = playerEntry;

        else
            playerListDic.Add(newPlayer.ActorNumber, playerEntry);


        //���� ����
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