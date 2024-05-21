using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class UserInfoCard : MonoBehaviour
{
    private void Awake()
    {
        myInfo = transform.Find("MyInfo");
        teamInfos = transform.Find("TeamInfos");
        enemyInfos = transform.Find("EnemyInfos");
    }

    public void Init()
    {
        InitMyInfo();
        InitTeamInfos();
        InitEnemyInfos();
    }

    #region MyInfo
    Transform myInfo;
    Slider hpBar;
    private void InitMyInfo()
    {
        myInfo.Find("NameLabel").GetComponent<TextMeshProUGUI>().text 
            = PhotonNetwork.LocalPlayer.NickName;
        hpBar = myInfo.Find("HPBar").GetComponent<Slider>();

        SetClass();
        //SetHpBar();
    }

    public void SetClass()
    {
        Player player = PhotonNetwork.LocalPlayer;
        PlayerClass playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);
        myInfo.Find("ClassImage").GetComponent<Image>().sprite 
            = GameManager.Instance.classList[(int)playerClass].classIcon;
    }
    public void SetHpBar()
    {
        PlayerAttack playerAttack = GameManager.Instance.playerAttack;
        float amount = playerAttack.HpAmount;
        hpBar.value = amount;
    }
    #endregion

    #region TeamInfo
    Transform teamInfos;
    GameObject teamPrefab;
    Dictionary<int, Transform> teamDic = new Dictionary<int, Transform>();
    Dictionary<int, Slider> teamHpDic = new Dictionary<int, Slider>();
    private void InitTeamInfos()
    {
        teamDic.Clear();
        teamHpDic.Clear();
        foreach (Transform t in teamInfos.transform)
        {
            Destroy(t.gameObject);
        }

        PhotonTeam myTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam();

        if(PhotonNetwork.LocalPlayer.TryGetTeamMates(out Player[] teamMates))
        {
            foreach(Player member in teamMates)
            {
                Transform teamInfo = Instantiate(teamPrefab, teamInfos).transform;
                teamInfo.Find("NameLabel").GetComponent<TextMeshProUGUI>().text 
                    = member.NickName;
                teamDic.Add(member.ActorNumber, teamInfo);
                SetTeamClass(member);
                //SetTeamHp(member);
            }
        }
    }
    public void SetTeamClass(Player player)
    {
        int num = (int)player.CustomProperties["Class"];

        if (num == -1)
        {
            teamDic[player.ActorNumber].Find("ClassImage").GetComponent<Image>().sprite
            = null;
        }
        else
        {
            teamDic[player.ActorNumber].Find("ClassImage").GetComponent<Image>().sprite
            = GameManager.Instance.classList[num].classIcon;
        }
    }
    public void SetTeamHp(Player player)
    {
        float amount = 0;
        //스폰되어있는 상태면
        if (SpawnManager.Instance.spawnedPlayers.ContainsKey(player.ActorNumber))
        {
            PlayerAttack playerAttack
            = SpawnManager.Instance.spawnedPlayers[player.ActorNumber].GetComponent<PlayerAttack>();
            amount = playerAttack.HpAmount;
        }

        teamHpDic[player.ActorNumber].value = amount;
    }
    #endregion

    #region EnemyInfo
    Transform enemyInfos;
    GameObject enemyPrefab;
    Dictionary<int, Transform> enemyDic = new Dictionary<int, Transform>();
    Dictionary<int, Slider> enemyHpDic = new Dictionary<int, Slider>();
    private void InitEnemyInfos()
    {
        enemyDic.Clear();
        enemyHpDic.Clear();
        foreach (Transform t in enemyInfos.transform)
        {
            Destroy(t.gameObject);
        }

        string myTeam = PhotonNetwork.LocalPlayer.GetPhotonTeam().Name;
        string enemyTeam = myTeam == "Blue" ? "Red" : "Blue";

        if(PhotonTeamsManager.Instance.TryGetTeamMembers(enemyTeam, out Player[] members))
        {
            foreach (Player member in members)
            {
                Transform enemyInfo = Instantiate(enemyPrefab, teamInfos).transform;
                enemyInfo.Find("NameLabel").GetComponent<TextMeshProUGUI>().text 
                    = member.NickName;
                teamDic.Add(member.ActorNumber, enemyInfo);
                SetEnemyClass(member);
                //SetEnemyHp(member);
            }
        }
    }
    public void SetEnemyClass(Player player)
    {
        int num = (int) player.CustomProperties["Class"];

        if(num == -1)
        {
            enemyDic[player.ActorNumber].Find("ClassImage").GetComponent<Image>().sprite
            = null;
        }
        else
        {
            enemyDic[player.ActorNumber].Find("ClassImage").GetComponent<Image>().sprite
            = GameManager.Instance.classList[num].classIcon;
        }
    }
    public void SetEnemyHp(Player player)
    {
        float amount = 0;
        if (SpawnManager.Instance.spawnedPlayers.ContainsKey(player.ActorNumber))
        {
            PlayerAttack playerAttack
            = SpawnManager.Instance.spawnedPlayers[player.ActorNumber].GetComponent<PlayerAttack>();
            amount = playerAttack.HpAmount;
        }

        enemyHpDic[player.ActorNumber].value = amount;
    }
    #endregion
}
