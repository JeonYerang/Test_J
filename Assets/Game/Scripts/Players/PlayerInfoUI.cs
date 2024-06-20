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
        SetNameLabel(playerInfo.PlayerName);
        SetOutLineColor(playerInfo.Team);
        SetClassIcon(playerInfo.playerClass);
    }

    public void SetNameLabel(string name)
    {
        nameLabel.text = name;
    }

    public void SetOutLineColor(string team)
    {
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

    public void SetClassIcon(PlayerClass playerClass)
    {
        Sprite classIcon = ClassManager.Instance.classList[(int)playerClass].classIcon;
        this.classIcon.sprite = classIcon;
    }

    public void SetHpBar(object sender, Player player)
    {
        float amount = playerAttack.HpAmount;
        hpBar.value = amount;
    }
}
