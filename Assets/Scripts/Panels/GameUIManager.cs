using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameUIManager : MonoBehaviourPunCallbacks
{
    public ClassSelectPanel classSelectPanel;

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
