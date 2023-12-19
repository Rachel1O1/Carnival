using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class ItemWorld : NetworkBehaviour
{   
    private SpriteRenderer spriteRenderer;
    private TextMeshPro textMeshPro;

    public bool mainCollide;

    [SyncVar(hook = nameof(OnChangeItem))]
    [SerializeField] private Item item;

    void OnChangeItem(Item oldItem, Item newItem)
    {
        SetItem(newItem);
    }

    private void Awake()
    {
        mainCollide = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("amountText").GetComponent<TextMeshPro>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1)
        {
            textMeshPro.SetText("" + item.amount);
        } else {
            textMeshPro.SetText("");
        }
    }

    [ClientRpc] //here because sync items to stack wasn't work with just SetItem as the amount wouldn't sync on all ends so this makes it for sure sets the item on all clients (including host)
    private void SetItemOverride(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
        if (item.amount > 1)
        {
            textMeshPro.SetText("" + item.amount);
        } else {
            textMeshPro.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    //Lets the server host do all the work for if items on the ground should stack or not and if so how
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (isServer)
        {
            ItemWorld itemWorldOther = collider.GetComponent<ItemWorld>();
            if (itemWorldOther != null)
            {
                if ((itemWorldOther.GetItem().itemType == this.item.itemType) && (this.item.isStackable()))
                {
                    mainCollide = true;
                    if (!itemWorldOther.mainCollide)
                    {
                        this.item.amount += itemWorldOther.GetItem().amount;
                        SetItemOverride(this.item);
                        StartCoroutine(WaitForHalf());
                        itemWorldOther.DestroySelf();
                    } else {
                        mainCollide = false;
                    }
                }
            }
        }
    }

    IEnumerator WaitForHalf()
    {
        yield return new WaitForSeconds(0.1f);
        mainCollide = false;
    }
}
