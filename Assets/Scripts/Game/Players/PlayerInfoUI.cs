using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameLabel;
    [SerializeField]
    Image classIcon;
    [SerializeField]
    Slider hpBar;
    [SerializeField]
    Outline outline;

    public void Init(string playerName, string team, PlayerClass playerClass)
    {
        SetNameLabel(playerName);
        SetOutLineColor(team);
        SetClassIcon(GameManager.Instance.classList[(int)playerClass].classIcon);
    }

    public void SetNameLabel(string name)
    {
        nameLabel.text = name;
    }

    public void SetOutLineColor(string team)
    {
        switch(team)
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

    public void SetClassIcon(Sprite classIcon)
    {
        this.classIcon.sprite = classIcon;
    }

    public void SetHpBar(int amount)
    {
        hpBar.value = amount;
    }
}
