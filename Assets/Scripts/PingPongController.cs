using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PingPongController : NetworkBehaviour
{
    [SerializeField] private Transform bar1;
    [SerializeField] private Transform bar2;

    [SyncVar(hook = nameof(OnChangePlayer1))]
    [SerializeField] private string player1;
    [SyncVar(hook = nameof(OnChangePlayer2))]
    [SerializeField] private string player2;

    [SerializeField] private TextMeshProUGUI displayP1;
    [SerializeField] private TextMeshProUGUI displayP2;

    [SerializeField] private Camera gameCamera;

    void Start()
    {
        player1 = null;
        player2 = null;
    }

    public Camera getCamera()
    {
        return gameCamera;
    }

    void OnChangePlayer1(string oldPlayer1, string newPlayer1)
    {
        setPlayer1(newPlayer1);
    }

    void OnChangePlayer2(string oldPlayer2, string newPlayer2)
    {
        setPlayer2(newPlayer2);
    }

    public string getPlayer1()
    {
        return player1;
    }

    public string getPlayer2()
    {
        return player2;
    }

    public void setPlayer1(string newPlayer)
    {
        player1 = newPlayer;
    }

    public void setPlayer2(string newPlayer)
    {
        player2 = newPlayer;
    }

    [Command(requiresAuthority = false)]
    void setPlayer1CMD(string newPlayer)
    {
        setPlayer1(newPlayer);
        refreshNames();
    }

    [Command(requiresAuthority = false)]
    void setPlayer2CMD(string newPlayer)
    {
        setPlayer2(newPlayer);
        refreshNames();
    }

    public bool waitingForPlayers()
    {
        return ((player1 == null) || (player2 == null));
    }

    public void addPlayer(string newPlayer)
    {
        if (player1 == null)
        {
            setPlayer1CMD(newPlayer);
        } else {
            if (player2 == null)
            {
                setPlayer2CMD(newPlayer);
            } else {
                Debug.Log("Full Game!");
            }
        }
    }

    public void removePlayer(string leavingPlayer)
    {
        if (player1 == leavingPlayer)
        {
            setPlayer1CMD(null);
        } else {
            if (player2 == leavingPlayer)
            {
                setPlayer2CMD(null);
            } else {
                Debug.Log("No Matching Player");
            }
        }
    }

    private void refreshNames()
    {
        if (player1 == null)
        {
            displayP1.SetText("waiting...");
        } else {
            displayP1.SetText(player1);
        }
        if (player2 == null)
        {
            displayP2.SetText("waiting...");
        } else {
            displayP2.SetText(player2);
        }
    }
}
