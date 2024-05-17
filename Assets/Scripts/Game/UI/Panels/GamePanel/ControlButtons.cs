using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtons : MonoBehaviour
{
    Button[] skillButtons = new Button[2];
    Button jumpButton;

    private void Awake()
    {
        InitSkillButtons();
        InitJumpButton();
    }

    private void InitSkillButtons()
    {
        Transform buttonsTransform = transform.Find("SkillButtons");

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i] = buttonsTransform.GetChild(i).GetComponent<Button>();
        }
    }

    private void InitJumpButton()
    {
        jumpButton = transform.Find("JumpButton").GetComponent<Button>();
    }
}
