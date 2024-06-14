using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    int index;

    Button skillButton;

    Image icon;
    Image coolIndicator;
    Image chargeIndicator;

    private void Awake()
    {
        skillButton = GetComponent<Button>();

        icon = transform.Find("Icon").GetComponent<Image>();
        coolIndicator = transform.Find("CoolIndicator").GetComponent<Image>();
        //chargeIndicator = transform.Find("ChargeIndicator").GetComponent<Image>();
    }

    public void InitIndex(int index)
    {
        this.index = index;
    }

    public void SetSkill(SkillSet skillData)
    {
        icon.sprite = skillData.icon;
        skillButton.onClick.AddListener(OnClickSkillButton);
    }

    public void ResetSkill()
    {
        icon.sprite = null;
        skillButton.onClick.RemoveAllListeners();
    }

    private void OnClickSkillButton()
    {
        SkillManager.Instance.TryUsingSkill(index);
    }

    public void Emphasize() //쿨타임 종료, 차징 카운트 올랐을시 강조
    {

    }

    //condition
    public void ShowCoolTime(float amount)
    {
        if(amount <= 0)
        {
            coolIndicator.gameObject.SetActive(false);
            Emphasize();
        }
        else
        {
            if (!coolIndicator.gameObject.activeSelf)
                coolIndicator.gameObject.SetActive(true);

            coolIndicator.fillAmount = amount;
        }
    }

    public void ShowRemainCount(float amount)
    {
        coolIndicator.fillAmount = amount;
    }

    public void StartShowChargeTime(float amount)
    {
        chargeIndicator.fillAmount = amount;
    }
}
