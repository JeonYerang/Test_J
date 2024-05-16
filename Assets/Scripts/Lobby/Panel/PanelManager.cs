using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    public LoginPanel loginPanel;
    public LobbyPanel lobbyPanel;
    public RoomPanel roomPanel;
    public UpdateInfoPanel updatePanel;

    Dictionary<string, GameObject> panelDic;

    private void Awake()
    {
        Instance = this;

        panelDic = new Dictionary<string, GameObject>()
        {
            { "Login", loginPanel.gameObject },
            { "Lobby", lobbyPanel.gameObject },
            { "Update", updatePanel.gameObject },
            { "Room", roomPanel.gameObject }
        };
    }

    private void Start()
    {
        PanelOpen("Login");
    }

    public void PanelOpen(string panelName)
    {
        foreach(var panel in panelDic)
        {
            panel.Value.SetActive(panel.Key == panelName);
        }
    }
}
