using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInfo : MonoBehaviour
{
    public Player player;
    PhotonView pv;
    public bool IsMine { get { return pv.IsMine; } }

    Camera playerCam;
    GameObject renderObj;

    public string PlayerName {  get; private set; }
    public string Team {  get; private set; }
    public bool IsTeam
    {
        get { return PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == Team; }
    }
    public PlayerClass playerClass { get; private set; }

    [SerializeField]
    PlayerInfoUI playerInfoUI;

    PlayerAttack playerAttack;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerCam = transform.Find("PlayerCam").GetComponent<Camera>();
        renderObj = transform.Find("Renderer").gameObject;
    }

    private void OnEnable()
    {
        player = pv.Owner;

        if(PhotonNetwork.IsConnected)
            SetInfo();
    }

    public void SetInfo()
    {
        PlayerName = player.NickName;
        Team = player.GetPhotonTeam().Name;
        playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);

        playerInfoUI.Init(this);

        if (pv.IsMine) //카메라 설정
        {
            renderObj.layer = LayerMask.NameToLayer("Me");
            playerCam.gameObject.SetActive(true);
        }

        playerAttack.SetClass(playerClass);
    }
}
