using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIinventory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField]private Transform itemSlotTemplate;
    [SerializeField]private RectTransform currentSlotBox;
    [SerializeField]private Transform holdingStuff;
    private int currentSlot; //0-9 for 1-9,0
    private GameObject player;
    private Transform messageHolder;
    private Transform messageTemplate;
    private bool bootup;

    private void Awake() {
        bootup = false;
        currentSlot = 0;
        holdingStuff = transform.Find("holding");
        itemSlotContainer = transform.Find("itemSlotHolder");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        currentSlotBox = transform.Find("slotSelected").GetComponent<RectTransform>();
        messageHolder = transform.Find("messageHolder");
        messageTemplate = messageHolder.Find("messageTemplate");
        bootup = true;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        try {
            if (bootup) {}
        } catch (Exception e)
        {
            Debug.Log(e);
            bootup = false;
        }
        if (!bootup)
        {
            currentSlot = 0;
            itemSlotContainer = transform.Find("itemSlotHolder");
            itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
            currentSlotBox = GameObject.Find("slotSelected").GetComponent<RectTransform>();
            messageHolder = transform.Find("messageHolder");
            messageTemplate = messageHolder.Find("messageTemplate");
            bootup = true;
        }

        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        inventory.OnTicketAmountChanged += Inventory_OnTicketAmountChanged;
        RefreshInventoryItems();
        RefreshSelectedItem();
        RefreshTicketDisplay();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void Inventory_OnTicketAmountChanged(object sender, System.EventArgs e)
    {
        RefreshTicketDisplay();
    }

    public int getCurrentSlot()
    {
        return currentSlot;
    }

    public void setCurrentSlot(int inputInt)
    {
        currentSlot = inputInt;
        RefreshSelectedItem();
    }

    public void useCurrent()
    {
        int listLen = inventory.GetItemList().Count;
        if (listLen > currentSlot)
        {
            Item currentItem = inventory.GetItemList()[currentSlot];
            inventory.UseItem(currentItem);
        } else {
            nothingHeldMessage();
        }
    }

    public void dropCurrent(PlayerMovement pmMaster)
    {
        int listLen = inventory.GetItemList().Count;
        if (listLen > currentSlot)
        {
            Item currentItem = inventory.GetItemList()[currentSlot];
            Item duplicateItem = new Item { itemType = currentItem.itemType, amount = currentItem.amount };
            inventory.RemoveItem(currentItem);
            pmMaster.DropItemCMD(player.GetComponent<Transform>().position, duplicateItem);
        } else {
            nothingHeldMessage();
        }
    }

    public void dropOneCurrent(PlayerMovement pmMaster)
    {
        int listLen = inventory.GetItemList().Count;
        if (listLen > currentSlot)
        {
            Item currentItem = inventory.GetItemList()[currentSlot];
            if (currentItem.amount > 1)
            {
                Item duplicateItem = new Item { itemType = currentItem.itemType, amount = 1 };
                inventory.RemoveItem(duplicateItem);
                pmMaster.DropItemCMD(player.GetComponent<Transform>().position, duplicateItem);
            } else {
                dropCurrent(pmMaster);
            }
        } else {
            nothingHeldMessage();
        }
    }

    private void RefreshTicketDisplay()
    {
        TextMeshProUGUI uiText = transform.Find("ticketDisplay").GetComponent<TextMeshProUGUI>();
        uiText.SetText("" + inventory.getTicketTotal());
    }

    private void RefreshSelectedItem()
    {
        float itemSlotCellSize = 41f;
        currentSlotBox.anchoredPosition = new Vector2((currentSlot * itemSlotCellSize) - 185f, -159f);
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        float itemSlotCellSize = 41f;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true); //makes it visible

            itemSlotRectTransform.anchoredPosition = new Vector2((x * itemSlotCellSize) - 185f, 0);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText("" + item.amount);
            } else {
                uiText.SetText("");
            }

            x++;
        }
    }

    public void makeNewMessage(string inputText)
    {
        newMessage(inputText);
    }

    private void nothingHeldMessage()
    {
        newMessage("You Are Holding Nothing");
    }

    private void newMessage(string inputText)
    {
        RectTransform newMessageTransform = Instantiate(messageTemplate, messageHolder).GetComponent<RectTransform>();
        newMessageTransform.gameObject.SetActive(true); //makes it visible
        TextMeshProUGUI uiText = newMessageTransform.Find("messageText").GetComponent<TextMeshProUGUI>();
        uiText.SetText(inputText);
        StartCoroutine(MessageFade(newMessageTransform));
    }

    IEnumerator MessageFade(RectTransform inputTransform)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(0.1f);
            float subMe = 1f - (.05f * ((float)i));
            foreach (Transform child in inputTransform)
            {
                child.gameObject.GetComponent<CanvasRenderer>().SetAlpha(subMe);
            }
            inputTransform.Translate(Vector3.up / 7);
        }
        Destroy(inputTransform.gameObject);
    }
}
