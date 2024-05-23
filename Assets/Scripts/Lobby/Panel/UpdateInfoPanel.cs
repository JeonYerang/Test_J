using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateInfoPanel : MonoBehaviour
{
    public InputField newNameInput;
    public Button confirmButton;
    public Button cancelButton;
    public Text confirmText;
    public Text warningText;

    private void Awake()
    {
        cancelButton.onClick.AddListener(OnCancelButtonClick);
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnEnable()
    {
        newNameInput.text = PhotonNetwork.NickName;
        //confirmText.SetActive(false);
        //warningText.SetActive(false);
        confirmText.color = 
            new Color(confirmText.color.r, confirmText.color.g, confirmText.color.b, 0);
        warningText.color =
            new Color(warningText.color.r, warningText.color.g, warningText.color.b, 0);
    }

    private void OnConfirmButtonClick()
    {
        if (string.IsNullOrEmpty(newNameInput.text))
        {
            if(confirmTextCoroutine != null)
                confirmTextCoroutine = null;
            confirmTextCoroutine = StartCoroutine(ShowTextCoroutine(warningText));
            return;
        }

        PhotonNetwork.NickName = newNameInput.text;

        if (warningTextCoroutine != null)
            warningTextCoroutine = null;
        warningTextCoroutine = StartCoroutine(ShowTextCoroutine(confirmText));
    }

    Coroutine confirmTextCoroutine = null;
    Coroutine warningTextCoroutine = null;
    IEnumerator ShowTextCoroutine(Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1)
        {
            text.color = 
                new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime * 3f));
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (text.color.a > 0)
        {
            text.color = 
                new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * 3f));
            yield return null;
        }
    }

    private void OnCancelButtonClick()
    {
        PanelManager.Instance.PanelOpen("Lobby");
    }
}
