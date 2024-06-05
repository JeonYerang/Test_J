using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    public InputField nickNameInput;
    public Button loginButton;
    public Button exitButton;
    public GameObject warningText;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnEnable()
    {
        nickNameInput.text = $"PLAYER{Random.Range(0, 99)}";
        nickNameInput.interactable = true;
        loginButton.interactable = true;
        warningText.SetActive(false);
    }

    private void OnLoginButtonClick()
    {
        if (string.IsNullOrEmpty(nickNameInput.text))
        {
            warningText.SetActive(true);
            return;
        }

        //PhotonNetwork.AuthValues = new Photon.Realtime.AuthenticationValues();
        PhotonNetwork.NickName = nickNameInput.text;

        nickNameInput.interactable = false;
        loginButton.interactable = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnExitButtonClick()
    {
        //게임을 종료하시겠습니까?
        Application.Quit();
    }
}
