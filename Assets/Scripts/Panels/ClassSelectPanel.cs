using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ClassSelectPanel : MonoBehaviour
{
    Text countDownText;

    Transform selectsParent;
    GameObject selectaleClassPrefab;

    Transform playerListParent;
    GameObject playerEntryPrefab;

    Button ConfirmButton;

    public ToggleGroup selectToggleGroup;
    private List<Toggle> selectToggles = new List<Toggle>();

    public Dictionary<int, GameObject> playerListDic = new Dictionary<int, GameObject>();
    public Dictionary<int, int> selectDic = new Dictionary<int, int>(); //<actorNum, select>

    private void Awake()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            selectDic.Add(player.ActorNumber, -1);
        }
        
    }

    public void InitPlayerList()
    {
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            GameObject playerEntry = Instantiate(playerEntryPrefab, playerListParent);

            if (playerEntry.transform.Find("NameLabel").TryGetComponent(out Text nameText))
                nameText.text = player.NickName;

            if (playerEntry.transform.Find("SelectLabel").TryGetComponent(out Text selectText))
                selectText.text = ((PlayerClass)((int)player.CustomProperties["Class"])).ToString();

            playerListDic.Add(player.ActorNumber, playerEntry);
        }
    }

    public void InitSelectList()
    {
        foreach (PlayerClass playerClass in Enum.GetValues(typeof(PlayerClass)))
        {
            GameObject selectElement = Instantiate(selectaleClassPrefab, selectsParent);

            if (selectElement.transform.Find("ClassLabel").TryGetComponent(out Text classText))
                classText.text = playerClass.ToString();

            Toggle addToggle = selectElement.transform.GetComponent<Toggle>();
            selectToggles.Add(addToggle);
        }
    }

    public void SelectClass(int select)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable customProps = localPlayer.CustomProperties;

        customProps["Class"] = select;
        localPlayer.SetCustomProperties(customProps);
    }

    public void SetSelect(int actorNum, int select)
    {
        PlayerClass selectedClass = (PlayerClass)select;

    }
}
