using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ItemWorldSpawner : NetworkBehaviour
{
    public Item item;

//has a timer because needs ItemAssets Instance to be init first and Network to set up
    /*private void Awake()
    {
        StartCoroutine(WaitForHalf());
    }

    IEnumerator WaitForHalf()
    {
        yield return new WaitForSeconds(0.2f);
        CmdSIW(transform.position, item);
    }

    [Command(requiresAuthority = false)]
    private void CmdSIW(Vector3 position, Item item)
    {
        SpawnItemWorld(position, item);
    }

    [ClientRpc]
    private void SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);
        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        //NetworkIdentity ni = itemWorld.gameObject.AddComponent<NetworkIdentity>();
        NetworkServer.Spawn(transform.gameObject);
        Destroy(gameObject);
    }*/
}
