using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    int index;

    Button skillButton;

    Image icon;
    Image coolIndicator;
    Image chargeIndicator;
    GameObject emphasizeIndicator;

    bool isChargingButton;

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
        icon.sprite = skillData.icon;

        if (skillData.castType == SkillCastType.Charge)
            isChargingButton = true;
        else
            isChargingButton = false;

        skillButton.onClick.AddListener(OnClick);
    }

    public void ResetSkill()
    {
        icon.sprite = null;
        skillButton.onClick.RemoveAllListeners();
    }

    #region Click Event
    private void OnClick()
    {
        if (isChargingButton)
        {
            longClickCheckCoroutine = StartCoroutine(LongClickCheck());
        }
    }

    float holdTime = 0.2f;
    private void OnLongClick()
    {
        
    }

    private void OnCancel()
    {
        if (!isChargingButton)
        {
            //shot
        }
        else
        {
            if (longClickCheckCoroutine == null)
                return; //shot
            else
                StopCoroutine(longClickCheckCoroutine);
        }
    }

    Coroutine longClickCheckCoroutine = null;
    IEnumerator LongClickCheck()
    {
        yield return new WaitForSeconds(holdTime);
        OnLongClick();
    }
    #endregion

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
