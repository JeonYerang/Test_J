using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIPanel : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerAttack playerAttack;

    [SerializeField]
    UserInfoCard userInfo;
    [SerializeField]
    ControlButtons controlButtons;

    [SerializeField]
    FixedJoystick joystick;
    public FixedJoystick Joystick {  get; private set; }

    public void OnEnable()
    {
        if(GameManager.Instance != null)
            PlayerSet();
    }

    public void PlayerSet()
    {
        playerMove = GameManager.Instance.playerMove;
        playerAttack = GameManager.Instance.playerAttack;

        userInfo.Init();
        controlButtons.Init();
    }
}
