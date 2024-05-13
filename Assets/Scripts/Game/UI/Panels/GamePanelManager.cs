using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GamePanelManager : MonoBehaviourPunCallbacks
{
    public static GamePanelManager Instance;

    public ClassSelectPanel classSelectPanel;

    private void Awake()
    {
        Instance = this;

        classSelectPanel.gameObject.SetActive(false);
    }

    public void ShowLoadingPanel()
    {

    }

    public void ShowSelectPanel()
    {
        classSelectPanel.gameObject.SetActive(true);
    }

    public void CloseSelectPanel()
    {
        classSelectPanel.gameObject.SetActive(false);
    }

    public void ShowDiedPanel()
    {

    }
}
