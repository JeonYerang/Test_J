using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    PlayerInfo playerInfo;
    PlayerAttack playerAttack;

    [SerializeField]
    TextMeshProUGUI nameLabel;
    [SerializeField]
    Image classIcon;
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Outline outline;

    private void Start()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerAttack.onChangedHp += SetHpBar;
    }

    public void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;

        SetNameLabel();
        SetOutLineColor();
        SetClassIcon();
    }

    public void SetNameLabel()
    {
        string name = playerInfo.PlayerName;
        nameLabel.text = name;
    }

    public void SetOutLineColor()
    {
        string team = playerInfo.Team;

        switch (team)
        {
            case "Blue":
                outline.OutlineColor = Color.blue;
                break;
            case "Red":
                outline.OutlineColor = Color.red;
                break;
            default:
                outline.OutlineColor = Color.white;
                break;
        }
    }

    public void SetClassIcon()
    {
        PlayerClass playerClass = playerInfo.PlayerClass;
        Sprite classIcon = GameManager.Instance.classList[(int)playerClass].classIcon;
        this.classIcon.sprite = classIcon;
    }

    public void SetHpBar(object sender, Player player)
    {
        float amount = playerAttack.HpAmount;
        hpBar.value = amount;
    }
}
