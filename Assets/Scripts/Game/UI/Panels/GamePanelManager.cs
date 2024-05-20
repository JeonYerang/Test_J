using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePanel
{
    Loading,
    Select,
    Game,
    Died,
    Result
}

public class GamePanelManager : MonoBehaviourPunCallbacks
{
    public static GamePanelManager Instance;

    public LoadingPanel loadingPanel;
    public SelectPanel selectPanel;
    public GameUIPanel gamePanel;
    public DiedPanel diedPanel;
    public ResultPanel resultPanel;

    Dictionary<string, GameObject> panelDic;

    private void Awake()
    {
        Instance = this;

        panelDic = new Dictionary<string, GameObject>()
        {
            { "Loading", loadingPanel.gameObject },
            { "Select", selectPanel.gameObject },
            { "Game", gamePanel.gameObject },
            { "Died", diedPanel.gameObject },
            { "Result", resultPanel.gameObject },
        };
    }

    private void Start()
    {
        foreach (var panel in panelDic)
        {
            panel.Value.SetActive(false);
        }
    }

    public void PanelOpen(GamePanel panelName)
    {
        foreach (var panel in panelDic)
        {
            panel.Value.SetActive(panel.Key == panelName.ToString());
        }
    }
}
