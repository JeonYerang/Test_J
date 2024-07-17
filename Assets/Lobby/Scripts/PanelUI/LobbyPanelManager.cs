using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanelManager : MonoBehaviour
{
    public static LobbyPanelManager Instance;

    public LoginPanel loginPanel;
    public UpdateInfoPanel updatePanel;
    public LobbyPanel lobbyPanel;
    public RoomPanel roomPanel;

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
