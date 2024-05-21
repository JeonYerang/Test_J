using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
    private string team;
    public string Team { get { return team; } }
    private PlayerClass playerClass;
    public PlayerClass PlayerClass { get {  return playerClass; } }

    [SerializeField]
    PlayerInfoUI playerInfoUI;

    [SerializeField]
    Camera playerCam;
    [SerializeField]
    RenderTexture profileTex;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        //playerCam = transform.Find("PlayerCam").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        player = pv.Owner;
        SetInfo();
    }

    public void SetInfo()
    {
        playerName = player.NickName;
        team = player.GetPhotonTeam().Name;
        playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);

        playerInfoUI.SetNameLabel(playerName);
        playerInfoUI.SetOutLineColor(team);
        playerInfoUI.SetClassIcon(GameManager.Instance.classList[(int)playerClass].classIcon);

        if(pv.IsMine)
            playerCam.targetTexture = profileTex;
    }
}
