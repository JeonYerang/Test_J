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
    public GameObject warningText;

    private void Awake()
    {
        cancelButton.onClick.AddListener(OnCancelButtonClick);
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    private void OnEnable()
    {
        newNameInput.text = PhotonNetwork.NickName;
        warningText.SetActive(false);
    }

    private void OnConfirmButtonClick()
    {
        if (string.IsNullOrEmpty(newNameInput.text))
        {
            warningText.SetActive(true);
            return;
        }

        PhotonNetwork.NickName = newNameInput.text;
    }

    private void OnCancelButtonClick()
    {
        PanelManager.Instance.PanelOpen("Lobby");
    }
}
