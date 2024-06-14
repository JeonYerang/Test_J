using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonsUI : MonoBehaviour
{
    [SerializeField]
    Transform skillButtonsParent;
    public SkillButton[] skillButtons;

    [SerializeField]
    Button jumpButton;

    //<button, skill index>
    Dictionary<Button, int> skillButtonDic = new Dictionary<Button, int>(); 

    private void Awake()
    {
        skillButtons = skillButtonsParent.GetComponentsInChildren<SkillButton>();

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].InitIndex(i);
        }
    }

    public void InitSkillButtons(SkillSet[] skillSets)
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
}
