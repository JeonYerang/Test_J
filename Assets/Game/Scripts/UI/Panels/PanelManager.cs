using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelName //�ν����Ϳ��� Panels�� ��������� �����ϵ��� ���ϳ�?
{
    Select,
    Died,
    Result
}

public class PanelManager : MonoBehaviourPunCallbacks
{
    public static PanelManager Instance;

    public Panel[] panels;

    Dictionary<string, Panel> panelDic = new Dictionary<string, Panel>();

    private void Awake()
    {
        Instance = this;

        foreach(var panel in panels)
        {
            panel.Init();
            panelDic.Add(panel.panelName, panel);
        }

        AllPanelClose();
    }

    public Panel GetPanel(PanelName panelName)
    {
        string name = panelName.ToString();
        if (panelDic.ContainsKey(name))
        {
            return panelDic[name];
        }

        return null;
    }

    public void PanelOpen(PanelName panelName)
    {
        foreach (var panel in panelDic)
        {
            panel.Value.gameObject.SetActive(panel.Key == panelName.ToString());
        }
    }

    public void AllPanelClose()
    {
        foreach (var panel in panelDic.Values)
        {
            panel.gameObject.SetActive(false);
        }
    }
}
