using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIPanel : MonoBehaviour
{
    public UserInfoUI UserInfo {  get; private set; }
    public ScoreUI Score { get; private set; }
    public SkillButtonUI SkillButtons { get; private set; }
    public FixedJoystick Joystick { get; private set; }

    private void Awake()
    {
        UserInfo = transform.Find("UsersUI").GetComponent<UserInfoUI>();
        Score = transform.Find("ScoreUI").GetComponent<ScoreUI>();
        SkillButtons = transform.Find("SkillButtons").GetComponent<SkillButtonUI>();
        Joystick = transform.Find("FixedJoystick").GetComponent<FixedJoystick>();
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
            StartCoroutine(WaitPlayerCoroutine());
    }

    private IEnumerator WaitPlayerCoroutine()
    {
        yield return new WaitUntil(
            () => GameManager.Instance.playerMove != null);
        //&& GameManager.Instance.playerAttack != null);

        //게임매니저에 플레이어가 세팅되면 패널을 초기화함
        InitPanel();
    }

    private void InitPanel()
    {
        UserInfo.Init();
        SkillButtons.Init();
    }
}
