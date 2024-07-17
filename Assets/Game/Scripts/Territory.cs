using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Territory : MonoBehaviour
{
    string CurrentTeam;
    [SerializeField]
    Material crystalMat;

    Vector2[] colorVectors = new Vector2[3];
    List<Player> redPlayers = new List<Player>();
    List<Player> bluePlayers = new List<Player>();

    PhotonView pv;
    [SerializeField]
    ScoreUI scoreUI;
    [SerializeField]
    GameObject occupiedCountRing;

    private void Awake()
    {
        Vector2 violet = new Vector2(2.9f, 0);
        Vector2 blue = new Vector2(2.75f, 0);
        Vector2 red = new Vector2(2.25f, 0);

        colorVectors[0] = violet;
        colorVectors[1] = blue;
        colorVectors[2] = red;

        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        crystalMat.mainTextureOffset = colorVectors[0];
        occupiedCountRing.SetActive(false);
    }

    #region Add & Remove Player
    [PunRPC]
    private void AddPlayer(Player player)
    {
        PhotonTeam team = player.GetPhotonTeam();
        switch (team.Name)
        {
            case "Red":
                redPlayers.Add(player);
                break;

            case "Blue":
                bluePlayers.Add(player);
                break;

            default:
                break;
        }

        CompareCount();
    }

    [PunRPC]
    private void RemovePlayer(Player player)
    {
        PhotonTeam team = player.GetPhotonTeam();
        switch (team.Name)
        {
            case "Red":
                redPlayers.Remove(player);
                break;

            case "Blue":
                bluePlayers.Remove(player);
                break;

            default:
                break;
        }

        CompareCount();
    }
    #endregion

    #region Check
    private void CompareCount()
    {
        int redCount = redPlayers.Count;
        int blueCount = bluePlayers.Count;

        string newTeam = null;

        if ((redCount == 0) ^ (blueCount == 0)) //����: �� �� ������ �ִ� ���
        {
            newTeam
                = redCount == 0 ? "Blue" : "Red";

            if (CurrentTeam != newTeam) //���� ���� ���̴� ���� ���� �����ϴ� ���� ���� ���� ���
            {
                if (countCoroutine != null)
                    StopCoroutine(countCoroutine);

                countCoroutine = StartCoroutine(OccupiedCountDown(newTeam));
            }
        }
        else //�߸�
        {
            if (countCoroutine != null)
                StopCoroutine(countCoroutine);

            Occupied();

            if (redCount == 0) //�ƹ��� ������
            {
                scoreUI.SetOccupiedText("");
            }
            else
            {
                scoreUI.SetOccupiedText("������");
            }
        }
    }
    #endregion

    #region Occupied
    Coroutine countCoroutine = null;
    int occupiedCount = 5;
    private IEnumerator OccupiedCountDown(string teamName)
    {
        scoreUI.SetOccupiedText($"{teamName}�� ���ɽõ���");

        occupiedCountRing.SetActive(true);

        float currentCount = occupiedCount;

        while (currentCount >= 0)
        {
            if (PhotonNetwork.IsMasterClient)
                pv.RPC("BroadCastRemainCount", RpcTarget.All, currentCount);

            yield return new WaitForSeconds(0.2f);

            currentCount -= 0.2f;
        }

        if (PhotonNetwork.IsMasterClient)
            pv.RPC("BroadCastCompleteCount", RpcTarget.All, teamName);
    }

    [PunRPC]
    public void BroadCastRemainCount(float count)
    {
        occupiedCountRing.transform.GetChild(0).GetComponent<Image>().fillAmount
                = 1 - (count / occupiedCount);
    }

    [PunRPC]
    public void BroadCastCompleteCount(string teamName)
    {
        Occupied(teamName);
    }

    private void Occupied(string teamName = null)
    {
        occupiedCountRing.SetActive(false);

        //���� ���� ���� �ڷ�ƾ
        if (colorCoroutine != null) StopCoroutine(colorCoroutine);
        colorCoroutine = StartCoroutine(ColorChanged(teamName));

        if (PhotonNetwork.IsMasterClient)
            if (getScoreCoroutine != null) StopCoroutine(getScoreCoroutine);

        if (teamName != null) //�߸��� �ƴ� ���
        {
            scoreUI.SetOccupiedText($"{teamName}�� ������");

            if(PhotonNetwork.IsMasterClient)
                getScoreCoroutine = StartCoroutine(GetScoreCoroutine(teamName));
        }
    }
    #endregion

    #region GetScore
    Coroutine getScoreCoroutine = null;
    IEnumerator GetScoreCoroutine(string teamName)
    {
        while (true)
        {
            pv.RPC("GetScore", RpcTarget.All, teamName, 1);
            yield return new WaitForSeconds(1);
        }
    }
    [PunRPC]
    private void GetScore(string teamName, int score)
    {
        GameManager.Instance.GetScore(teamName, score);
    }

    Coroutine colorCoroutine = null;
    IEnumerator ColorChanged(string teamName)
    {
        Vector2 target;
        switch (teamName)
        {
            case "Blue":
                target = colorVectors[1];
                break;

            case "Red":
                target = colorVectors[2];
                break;

            default:
                target = colorVectors[0];
                break;
        }

        Vector2 before = crystalMat.mainTextureOffset;
        Vector2 amount = new Vector2(0.01f, 1);

        if (before.x > target.x)
        {
            while (crystalMat.mainTextureOffset.x > target.x)
            {
                crystalMat.mainTextureOffset -= amount;
                yield return new WaitForSeconds(0.05f);
            }
            crystalMat.mainTextureOffset = target;
        }
        else
        {
            while (crystalMat.mainTextureOffset.x < target.x)
            {
                crystalMat.mainTextureOffset += amount;
                yield return new WaitForSeconds(0.05f);
            }
            crystalMat.mainTextureOffset = target;
        }
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<PhotonView>().Owner;
            if(pv.IsMine)
            {
                pv.RPC("AddPlayer", RpcTarget.All, player);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<PhotonView>().Owner;
            if (pv.IsMine)
            {
                pv.RPC("RemovePlayer", RpcTarget.All, player);
            }
        }
    }
}
