using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    PhotonView pv;

    Player player;
    private string playerName;
    public string PlayerName {  get { return playerName; } }
    private Team team;
    public Team Team { get { return team; } }
    private PlayerClass playerClass;
    public PlayerClass PlayerClass { get {  return playerClass; } }

    [SerializeField]
    PlayerInfoUI playerInfoUI;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        player = pv.Owner;
        SetInfo();
    }

    public void SetInfo()
    {
        playerName = player.NickName;
        team = (Team)((int)player.CustomProperties["Team"]);
        playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);

        playerInfoUI.SetNameLabel(playerName);
        playerInfoUI.SetOutLineColor(team);
    }
}
