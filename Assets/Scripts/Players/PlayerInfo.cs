using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public string playerName;

    public bool team;
    PlayerClass playerClass;

    public void SetInfo(string name, bool team)
    {
        this.playerName = name;
        this.team = team;
    }

    public void SelectClass(PlayerClass playerClass)
    {
        this.playerClass = playerClass;
    }

    public void DecisionClass()
    {
        gameObject.AddComponent(PlayerSpawnManager.Instance.classDic[playerClass].playerAttack.GetType());
    }
}
