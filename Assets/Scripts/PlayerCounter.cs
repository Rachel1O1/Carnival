using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCounter : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnChangeCount))]
    [SerializeField] int playerCount;

    void OnChangeCount(int oldPlayerCount, int newPlayerCount)
    {
        SetCount(newPlayerCount);
    }

    private void SetCount(int newPlayerCount)
    {
        playerCount = newPlayerCount;
    }

    [Command(requiresAuthority = false)]
    void SetCountCMD(int newNum)
    {
        SetCount(newNum);
    }

    public int addToCount()
    {
        int newNum = (playerCount + 1);
        SetCountCMD(newNum);
        return newNum;
    }
}
