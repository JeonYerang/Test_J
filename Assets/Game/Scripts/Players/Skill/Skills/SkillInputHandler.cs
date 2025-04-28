using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillInputHandler : MonoBehaviour
{
    PlayerAttack playerAttack;
    ButtonBinder buttonBinder;

    //input queue
    //input priority

    private void Awake()
    {
        KeyBinder.OnInputSkillKey += OnInput;
        ButtonBinder.OnInputSkillButton += OnInput;
    }

    public void OnInput(int index, bool isPressed)
    {
        
        print("SkillInputHandler±îÁö Àü´ÞµÊ");
        if (playerAttack == null)
            return;

        switch (playerAttack.GetSkillWithIndex(index).castType)
        {
            case SkillCastType.Charge:
                //buttonBinder.GetSKillButton(index).StartShowChargeTime;
                break;

            default:
                break;
        }

        if (isPressed)
            playerAttack.TryUsingSkill(index);

        else
            playerAttack.EndSKill(index);
    }
}
