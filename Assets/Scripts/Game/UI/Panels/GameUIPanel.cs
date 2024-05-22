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
    public UserInfoCard UserInfo { get { return userInfo; } }
    [SerializeField]
    ControlButtonArea controlButtons;
    public ControlButtonArea ControlButtons { get { return controlButtons; } }

    [SerializeField]
    FixedJoystick joystick;
    public FixedJoystick Joystick { get { return joystick; } }

    public void OnEnable()
    {
        if (GameManager.Instance != null)
            StartCoroutine(WaitPlayerCoroutine());
    }
    private IEnumerator WaitPlayerCoroutine()
    {
        yield return new WaitUntil(
            () => GameManager.Instance.playerMove != null);
            //&& GameManager.Instance.playerAttack != null);
        PlayerSet();
    }

    public void PlayerSet()
    {
        playerMove = GameManager.Instance.playerMove;
        playerAttack = GameManager.Instance.playerAttack;

        InitPanel();
    }

    private void InitPanel()
    {
        userInfo.Init();
        controlButtons.Init();
    }
}
