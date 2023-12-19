using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    public event EventHandler OnTicketAmountChanged;

    private List<Item> itemList;
    private Action<Item> useItemAction;

    private int ticketTotal;

    public Inventory(Action<Item> useItemAction)
    {
        ticketTotal = 0;
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
    }

    public bool spendTicket()
    {
        if (ticketTotal > 0)
        {
            ticketTotal--;
            OnTicketAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        } else {
            return false;
        }
    }

    public int getTicketTotal()
    {
        return ticketTotal;
    }

    public bool AddItem(Item item) {
        if (item.isStackable())
        {
            if (item.itemType == Item.ItemType.Ticket)
            {
                ticketTotal += item.amount;
                OnTicketAmountChanged?.Invoke(this, EventArgs.Empty);
            } else {
                bool itemAlreadyInInventory = false;
                foreach (Item inventoryItem in itemList)
                {
                    if (inventoryItem.itemType == item.itemType)
                    {
                        inventoryItem.amount += item.amount;
                        itemAlreadyInInventory = true;
                    }
                }
                if (!itemAlreadyInInventory)
                {
                    if (itemList.Count < 10)
                    {
                        itemList.Add(item);
                    } else {
                        return false;
                    }
                }
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
            }
            return true;
        } else {
            if (itemList.Count < 10)
            {
                itemList.Add(item);
                OnItemListChanged?.Invoke(this, EventArgs.Empty);
                return true;
            } else {
                return false;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        if (item.isStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        } else {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
