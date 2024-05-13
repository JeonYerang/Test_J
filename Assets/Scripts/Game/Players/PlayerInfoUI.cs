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

    public void SetNameLabel(string name)
    {
        nameLabel.text = name;
    }

    public void SetOutLineColor(Team team)
    {
        switch(team)
        {
            case Team.Blue:
                outline.OutlineColor = Color.blue;
                break;
            case Team.Red:
                outline.OutlineColor = Color.red;
                break;
            default:
                outline.OutlineColor = Color.white;
                break;
        }
    }

    public void SetClassIcon()
    {

    }
}
