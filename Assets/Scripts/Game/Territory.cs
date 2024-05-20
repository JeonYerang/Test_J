using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Territory : MonoBehaviour
{
    string CurrentTeam;
    [SerializeField]
    Material crystalMat;

    Vector2[] colorVectors = new Vector2[3];
    List<Player> redPlayers = new List<Player>();
    List<Player> bluePlayers = new List<Player>();

    PhotonView pv;

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
    }

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

    private void CompareCount()
    {
        if(redPlayers.Count == bluePlayers.Count)
        {
            Occupied();
        }
        else if(redPlayers.Count > bluePlayers.Count)
        {
            Occupied("Red");
        }
        else
        {
            Occupied("Blue");
        }
    }

    Coroutine countCoroutine = null;
    private IEnumerator OccupiedCountDown()
    {
        yield return null;
    }

    Coroutine colorCoroutine = null;
    private void Occupied(string teamName = null)
    {
        if (CurrentTeam == teamName)
        {
            return;
        }

        CurrentTeam = teamName;
        if (teamName != null) print($"{teamName}팀이 점령함");
        else print("중립");

        if(colorCoroutine != null) StopCoroutine(colorCoroutine);
        switch (teamName)
        {
            case "Blue":
                colorCoroutine = StartCoroutine(CrystalColorChanged(colorVectors[1]));
                break;

            case "Red":
                colorCoroutine = StartCoroutine(CrystalColorChanged(colorVectors[2]));
                break;

            default:
                colorCoroutine = StartCoroutine(CrystalColorChanged(colorVectors[0]));
                break;
        }
    }

    IEnumerator CrystalColorChanged(Vector2 target)
    {
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
