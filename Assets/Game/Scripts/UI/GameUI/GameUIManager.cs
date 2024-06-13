using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    public UserInfoUI UserInfo {  get; private set; }
    public ScoreUI Score { get; private set; }
    public SkillButtonsUI SkillButtons { get; private set; }
    public FixedJoystick Joystick { get; private set; }

    private void Awake()
    {
        Instance = this;

        UserInfo = transform.Find("UserInfoUI").GetComponent<UserInfoUI>();
        Score = transform.Find("ScoreCard").GetComponent<ScoreUI>();
        SkillButtons = transform.Find("SkillButtons").GetComponent<SkillButtonsUI>();
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

        //���ӸŴ����� �÷��̾ ���õǸ� �г��� �ʱ�ȭ��
        InitPanel();
    }

    private void InitPanel()
    {
        UserInfo.Init();
        SkillButtons.Init();
    }
}
