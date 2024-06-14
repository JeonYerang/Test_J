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

    Camera playerCam;
    GameObject renderObj;

    public string PlayerName {  get; private set; }
    public string Team {  get; private set; }
    public PlayerClass PlayerClass { get; private set; }

    [SerializeField]
    PlayerInfoUI playerInfoUI;

    public bool IsMine { get { return pv.IsMine; }}
    public bool IsTeam { 
        get { return PhotonNetwork.LocalPlayer.GetPhotonTeam().Name == Team; } }

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
        PlayerClass = (PlayerClass)((int)player.CustomProperties["Class"]);

        playerInfoUI.Init(this);

        if (pv.IsMine)
        {
            renderObj.layer = LayerMask.NameToLayer("Me");
            playerCam.gameObject.SetActive(true);
        }
    }

    public void SetClass()
    {
        PlayerClass = (PlayerClass)((int)player.CustomProperties["Class"]);
        playerInfoUI.SetClassIcon();
    }
}
