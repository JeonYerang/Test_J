using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameUIManager : MonoBehaviourPunCallbacks
{
    public static GameUIManager Instance;

    public ClassSelectPanel classSelectPanel;

    private void Awake()
    {
        Instance = this;

        classSelectPanel.gameObject.SetActive(false);
    }

    public void ShowLoadingImage()
    {

    }

    public void ShowSelectPanel()
    {
        classSelectPanel.gameObject.SetActive(true);
        classSelectPanel.InitPlayerList();
    }

    public void ShowDiedPanel()
    {

    }
}
