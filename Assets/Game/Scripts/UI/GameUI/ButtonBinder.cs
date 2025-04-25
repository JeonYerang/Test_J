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
    Dictionary<Button, int> skillButtonDic = new Dictionary<Button, int>();

    public static event Action<int, bool> OnInputSkillButton;

    private void Awake()
    {
        skillButtons = skillButtonParent.GetComponentsInChildren<SkillButton>();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].InitIndex(i);
        }
    }

    private void OnEnable()
    {
        SkillButton.OnInputButton += OnPointerDown;
    }

    private void OnDisable()
    {
        SkillButton.OnInputButton -= OnPointerDown;
    }

    public void InitSkillButtons(SkillData[] skillSets)
    {
        if (skillButtons.Length >= skillSets.Length)
        {
            for (int i = 0; i < skillSets.Length; i++)
            {
                skillButtons[i].SetSkill(skillSets[i]);
            }
        }
        else
        {
            for (int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i].SetSkill(skillSets[i]);
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

    public void OnPointerDown(int index, bool isPressed)
    {
        OnInputSkillButton?.Invoke(index, isPressed);
    }
}
