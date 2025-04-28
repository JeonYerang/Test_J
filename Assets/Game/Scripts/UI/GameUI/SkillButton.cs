using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//버튼이자 인디게이터...
//state로 관리? (default, charging, cool) 
public class SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    int index;
    SkillCastType buttonType;

    Button skillButton;

    Image icon;

    TextMeshProUGUI textIndicator;
    Image coolIndicator;
    Image chargeIndicator;
    GameObject emphasizeIndicator;

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

    public void SetSkill(SkillData skillData)
    {
        print("SetSkill");
        icon.sprite = skillData.icon;
    }

    public void ResetSkill()
    {
        icon.sprite = null;
        skillButton.onClick.RemoveAllListeners();
    }

    //Click Event
    public event Action<int, bool> OnInputCallback;
    public void OnPointerDown(PointerEventData eventData)
    {
        print($"Button{index}: 버튼 눌림");
        skillButton.interactable = false;
        OnInputCallback?.Invoke(index, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        skillButton.interactable = true;
        OnInputCallback?.Invoke(index, false);
    }

    public void ShowState()
    {
        if (buttonType == SkillCastType.Charge)
            StartShowChargeTime(0);
    }



    #region indicator
    public void ShowCoolTime(float amount)
    {
        if(amount <= 0)
        {
            coolIndicator.gameObject.SetActive(false);
            //Emphasize();
        }
        else
        {
            if (!coolIndicator.gameObject.activeSelf)
                coolIndicator.gameObject.SetActive(true);

            coolIndicator.fillAmount = amount;
            ShowText($"{amount}s");
        }
    }

    public void StartShowChargeTime(float amount)
    {
        chargeIndicator.fillAmount = amount;
    }

    public void ShowText(string str)
    {
        textIndicator.text = str;
    }

    #region Emphasize: 쿨타임 종료, 차징 완료 시 강조
    public void Emphasize() 
    {
        emphasizeIndicator.SetActive(true);
    }
    public void Emphasize(float sec)
    {
        emphasizeIndicator.SetActive(true);
        Invoke("StopEmphasize", sec);
    }
    public void StopEmphasize()
    {
        emphasizeIndicator.SetActive(false);
    }
    #endregion
    #endregion
}
