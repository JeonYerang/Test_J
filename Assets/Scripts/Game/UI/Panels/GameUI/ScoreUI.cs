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

    public void SetOccupiedText(string text)
    {
        occupiedTeamText.text = text;
    }

    public void SetScoreText(string TeamName, int score)
    {
        if(TeamName == "Blue")
            blueTeamScoreText.text = score.ToString();
        else if (TeamName == "Red")
            redTeamScoreText.text = score.ToString();
    }
}
