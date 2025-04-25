using System.Collections;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    int index;

    Button skillButton;

    TextMeshProUGUI numText;

    Image icon;
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
    public void OnPointerDown(PointerEventData eventData)
    {
        skillButton.interactable = false;

        print($"OnClick {index}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        skillButton.interactable = true;

        print($"OnCancel {index}");
    }

    #region Emphasize
    public void Emphasize() //쿨타임 종료, 차징 완료 시 강조
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

    //condition
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
