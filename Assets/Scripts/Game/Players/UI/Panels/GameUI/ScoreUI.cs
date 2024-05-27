using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI occupiedTeamText;
    [SerializeField] TextMeshProUGUI blueTeamScoreText;
    [SerializeField] TextMeshProUGUI redTeamScoreText;
    [SerializeField] TextMeshProUGUI systemText;

    private void Start()
    {
        GameManager.Instance.onGetScore += SetScoreText;
    }

    public void SetOccupiedText(string text)
    {
        occupiedTeamText.text = text;
    }

    public void SetScoreText(object sender, EventArgs e)
    {
        blueTeamScoreText.text = GameManager.Instance.BlueTeamScore.ToString();
        redTeamScoreText.text = GameManager.Instance.RedTeamScore.ToString();
    }

    public void SetSystemText(string text)
    {
        systemText.text = text;
    }
}
