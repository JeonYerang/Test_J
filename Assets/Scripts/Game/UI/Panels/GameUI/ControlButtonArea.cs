using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtonArea : MonoBehaviour
{
    PlayerMove playerMove;
    PlayerAttack playerAttack;

    Button[] skillButtons = new Button[2];
    Button jumpButton;

    public void Init()
    {
        playerMove = GameManager.Instance.playerMove;
        playerAttack = GameManager.Instance.playerAttack;
        InitJumpButton();
        InitSkillButtons();
    }

    private void InitSkillButtons()
    {
        Transform buttonsTransform = transform.Find("SkillButtons");

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i] = buttonsTransform.GetChild(i).GetComponent<Button>();
            skillButtons[i].onClick.AddListener(OnClickSkillButton);
        }
    }

    private void InitJumpButton()
    {
        jumpButton = transform.Find("JumpButton").GetComponent<Button>();
        jumpButton.onClick.AddListener(OnClickJumpButton);
    }

    private void OnClickSkillButton()
    {

    }

    private void OnClickJumpButton()
    {
        playerMove.OnJump();
    }
}
