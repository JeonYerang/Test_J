using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoCard : MonoBehaviour
{
    //유저 정보
    TextMeshProUGUI userName;
    Image playerImage;
    Image classImage;
    Slider hpBar;

    public void Init()
    {
        InitMyInfo();
    }

    public void InitMyInfo()
    {
        Transform myInfo = transform.Find("MyInfo");
        userName = myInfo.Find("NameLabel").GetComponent<TextMeshProUGUI>();
        playerImage = myInfo.Find("ProfileImage").GetComponent<Image>();
        classImage = myInfo.Find("ClassImage").GetComponent<Image>();
        hpBar = myInfo.Find("HPBar").GetComponent<Slider>();

        Player player = PhotonNetwork.LocalPlayer;
        userName.text = player.NickName;
        PlayerClass playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);
        classImage.sprite = GameManager.Instance.classList[(int)playerClass].classIcon;
    }

    public void SetHpBar()
    {

    }
}
