using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoArea : MonoBehaviour
{
    //public RawImage profileImage;
    public Text nameText;
    public Button updateButton;
    public Button logoutButton;

    private void Awake()
    {
        updateButton.onClick.AddListener(OnUpdateButtonClick);
        logoutButton.onClick.AddListener(OnLogoutButtonClick);
    }

    public void SetInfo()
    {
        nameText.text = PhotonNetwork.LocalPlayer.NickName;
    }

    private void OnUpdateButtonClick()
    {
        LobbyPanelManager.Instance.PanelOpen("Update");
    }

    private void OnLogoutButtonClick()
    {
        PhotonNetwork.Disconnect();
        LobbyPanelManager.Instance.PanelOpen("Login");
    }
}
