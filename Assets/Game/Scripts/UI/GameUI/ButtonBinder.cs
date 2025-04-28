using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData;

public class ButtonBinder : MonoBehaviour
{
    [SerializeField]
    Transform skillButtonParent;
    [SerializeField]
    SkillButton[] skillButtons;

    [SerializeField]
    Button jumpButton;

    //<button, skill index>
    //Dictionary<Button, int> skillButtonDic = new Dictionary<Button, int>();

    public static event Action<int, bool> OnInputSkillButton;

    private void Awake()
    {
        skillButtons = skillButtonParent.GetComponentsInChildren<SkillButton>();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].InitIndex(i);
        }
    }

    public void InitSkillButtons(SkillData[] skillSets)
    {
        if (skillButtons.Length >= skillSets.Length)
        {
            for (int i = 0; i < skillSets.Length; i++) //스킬 종류가 버튼 개수 보다 많으면
            {
                skillButtons[i].SetSkill(skillSets[i]);
                skillButtons[i].OnInputCallback += OnButtonInput;
            }
        }
        else
        {
            for (int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i].SetSkill(skillSets[i]);
                skillButtons[i].OnInputCallback += OnButtonInput;
            }
        }
    }

    public void InitJumpButton()
    {
        jumpButton.onClick.AddListener(OnClickJumpButton);
    }

    private void OnClickJumpButton()
    {
        if (GameManager.Instance.playerMove != null)
            GameManager.Instance.playerMove.OnJump();
    }

    public void OnButtonInput(int index, bool isPressed)
    {
        print("ButtonBinder: 버튼 눌림");
        OnInputSkillButton?.Invoke(index, isPressed);
    }

    public SkillButton GetSKillButton(int index)
    {
        return skillButtons[index];
    }
}
