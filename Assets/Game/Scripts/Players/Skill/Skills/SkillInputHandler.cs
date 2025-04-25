using System;
using UnityEngine;

public class SkillInputHandler : MonoBehaviour
{
    PlayerAttack playerAttack;

    private void Awake()
    {
        KeyBinder.OnInputSkillKey += OnInput;
        ButtonBinder.OnInputSkillButton += OnInput;
    }

    public void OnInput(int index, bool isPressed)
    {
        if (playerAttack == null)
            return;

        if (isPressed)
            playerAttack.TryUsingSkill(index);

        else
            playerAttack.EndSKill(index);
    }
}
