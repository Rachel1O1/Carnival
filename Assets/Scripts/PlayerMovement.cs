using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;
using System;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    public Rigidbody2D rb;

    private Vector2 moveDirection;
    private Inventory inventory;

    private int lastTouched;

    private float moveSpeed;
    [SerializeField] private UIinventory uiInventory;
    [SerializeField] private DialogueScript dialogueScript;
    [SerializeField] private Camera myCamera;

    private PingPongController pingPong;

    [SerializeField] private TMP_Text myNametag;

    [SyncVar(hook = nameof(OnChangeUsername))]
    private string username;

    private bool goodForThrow;

    private int inGame;
    //0 = none, 1 = ping pong,

    void OnChangeUsername(string oldUsername, string newUsername)
    {
        SetUsername(newUsername);
    }

    void SetUsername(string newName)
    {
        username = newName;
        myNametag.text = username;
    }

    [Command(requiresAuthority = false)]
    public void setupName(string newName)
    {
        SetUsername(newName);
    }

    void Awake()
    {
        username = "null";
        goodForThrow = true;
        moveSpeed = 5;
        gameObject.SetActive(true);
        inventory = new Inventory(UseItem);
        uiInventory.SetPlayer(gameObject);
        uiInventory.SetInventory(inventory);
        myCamera.enabled = true;
        StartCoroutine(WaitToSpawn());
        pingPong = GameObject.Find("PingPongStuff").GetComponent<PingPongController>();
        inGame = 0;
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(0.2f);
        if (isLocalPlayer)
        {
            GameObject[] playerCounter = GameObject.FindGameObjectsWithTag ("PlayerCounter");
            setupName("Player" + (playerCounter[0].GetComponent<PlayerCounter>().addToCount()));
        } else {
            myNametag.text = username;
        }
        GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");
        int count = 0;
        //Debug.Log("sl:" + spawners.Length);
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].activeInHierarchy)
            {
                SpawnItemWorldCMD(spawners[i].transform.position, spawners[i].GetComponent<ItemWorldSpawner>().item);
                Destroy(spawners[i].gameObject);
                count++;
            }
        }
    }

    [Command]
    public void SpawnItemWorldCMD(Vector3 position, Item item)
    {
        //if (!isLocalPlayer) { return; }
        Debug.Log("try spawn, local:" + isLocalPlayer + " and is server:" + isServer);
        GameObject gm = Instantiate(NetworkManager.singleton.spawnPrefabs[0], position + new Vector3(0f, -.1f, 0f), Quaternion.identity);
        ItemWorld itemWorld = gm.transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        NetworkServer.Spawn(gm);
    }

    [Command]
    public void DropItemCMD(Vector3 dropPosition, Item item)
    {
        Vector3 dropDir = new Vector3(0f, -.7f, 0f);
        Debug.Log("try spawn, local:" + isLocalPlayer + " and is server:" + isServer);
        GameObject gm = Instantiate(NetworkManager.singleton.spawnPrefabs[0], dropPosition + dropDir, Quaternion.identity);
        ItemWorld itemWorld = gm.transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        //itemWorld.GetComponent<Rigidbody2D>().AddForce(dropDir, ForceMode2D.Impulse);
        NetworkServer.Spawn(gm);
    }

    void Update()
    {
        //Debug.Log("ilp:" + isLocalPlayer);
        if (!isLocalPlayer) {
            myCamera.enabled = false;
        return; }

        //process inputs
        if (!dialogueScript.getMode())
        {
            switch (inGame)
            {
                case 0:
                    ProcessInputs();
                    break;
                case 1:
                    ProcessPingPongInputs();
                    break;
            }
        } else {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                dialogueScript.clickNext();
            }
            if (dialogueScript.getIsChoice())
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    //selected yes1
                    processYes();
                    dialogueScript.NextLine();
                }
                if (Input.GetKeyDown(KeyCode.Alpha0)) {
                    //selected no0
                    dialogueScript.NextLine();
                }
            } 
        }
    }

    private void processYes()
    {
        int talkScriptNum = dialogueScript.getTalkScriptNum();
        if (talkScriptNum == 1) {
            bool enoughTickets = inventory.spendTicket();
            if (enoughTickets)
            {
                Debug.Log("try pre-spawn from " + isLocalPlayer);
                DropItemCMD(transform.position,  new Item { itemType = Item.ItemType.FunnelCake, amount = 1 }); //ItemWorld.
            } else {
                uiInventory.makeNewMessage("Not Enough Tickets");
            }
        }
        if (talkScriptNum == 2) {
            bool enoughTickets = inventory.spendTicket();
            if (enoughTickets)
            {
                Debug.Log("try pre-spawn from " + isLocalPlayer);
                //SpawnItemWorldCMD
                DropItemCMD(transform.position,  new Item { itemType = Item.ItemType.CaramelApple, amount = 1 }); //ItemWorld.
            } else {
                uiInventory.makeNewMessage("Not Enough Tickets");
            }
        }
        if (talkScriptNum == 5) {
            //Ping Pong
            if (pingPong.waitingForPlayers())
            {
                inGame = 1;
                pingPong.addPlayer(username);
                pingPong.getCamera().enabled = true;
                myCamera.enabled = false;
            } else {
                uiInventory.makeNewMessage("Full Game!");
            }
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessPingPongInputs()
    {
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 pingPongDirections = new Vector2(0, moveY);
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            uiInventory.setCurrentSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            uiInventory.setCurrentSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            uiInventory.setCurrentSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            uiInventory.setCurrentSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            uiInventory.setCurrentSlot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            uiInventory.setCurrentSlot(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            uiInventory.setCurrentSlot(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            uiInventory.setCurrentSlot(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            uiInventory.setCurrentSlot(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            uiInventory.setCurrentSlot(9);
        }
        if (Input.GetMouseButtonUp(0)) {
            //Left Click
            uiInventory.useCurrent();
        }
        if ((Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.LeftShift)) || (Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.RightShift))) {
            //Shifted Right Click
            if (goodForThrow)
            {
                goodForThrow = false;
                uiInventory.dropOneCurrent(this);
                StartCoroutine(WaitForThrow());
            }
        } else if (Input.GetMouseButtonUp(1)) {
            //Right Click
            if (goodForThrow)
            {
                goodForThrow = false;
                uiInventory.dropCurrent(this);
                StartCoroutine(WaitForThrow());
            }
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private void UseItem(Item item)
    {
        bool result = true;
        switch (item.itemType)
        {
            //case Item.ItemType.Ticket:
            case Item.ItemType.Balloon:
                uiInventory.makeNewMessage("Popped Balloon Item");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Balloon, amount = 1 });
                break;
            //case Item.ItemType.BottleEmpty:
            case Item.ItemType.BottleFizzy:
                uiInventory.makeNewMessage("Drank Fizzy");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.BottleFizzy, amount = 1 });
                result = inventory.AddItem(new Item { itemType = Item.ItemType.BottleEmpty, amount = 1 });
                if (!result)
                {
                    uiInventory.makeNewMessage("Full Inventory");
                    DropItemCMD(transform.position, new Item { itemType = Item.ItemType.BottleEmpty, amount = 1 });
                }
                break;
            case Item.ItemType.BottleSoul:
                uiInventory.makeNewMessage("Freed Soul");
                inventory.RemoveItem(item);
                result = inventory.AddItem(new Item { itemType = Item.ItemType.BottleEmpty, amount = 1 });
                if (!result)
                {
                    uiInventory.makeNewMessage("Full Inventory");
                    DropItemCMD(transform.position, new Item { itemType = Item.ItemType.BottleEmpty, amount = 1 });
                }
                break;
            //case Item.ItemType.Can:
            case Item.ItemType.CaramelApple:
                uiInventory.makeNewMessage("Ate Caramel Apple");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.CaramelApple, amount = 1 });
                break;
            case Item.ItemType.Corndog:
                uiInventory.makeNewMessage("Ate Corndog");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Corndog, amount = 1 });
                break;
            case Item.ItemType.CottonCandy:
                uiInventory.makeNewMessage("Ate Cotton Candy");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.CottonCandy, amount = 1 });
                break;
            case Item.ItemType.Dart:
                uiInventory.makeNewMessage("Used Dart");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Dart, amount = 1 });
                break;
            case Item.ItemType.FishingRod:
                uiInventory.makeNewMessage("Cast Fishing Rod");
                break;
            case Item.ItemType.FunnelCake:
                uiInventory.makeNewMessage("Ate Funnel Cake");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.FunnelCake, amount = 1 });
                result = inventory.AddItem(new Item { itemType = Item.ItemType.Plate, amount = 1 });
                if (!result)
                {
                    uiInventory.makeNewMessage("Full Inventory");
                    DropItemCMD(transform.position, new Item { itemType = Item.ItemType.Plate, amount = 1 });
                }
                break;
            case Item.ItemType.HotdogPlain:
                uiInventory.makeNewMessage("Ate Tasty Hotdog");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HotdogPlain, amount = 1 });
                break;
            case Item.ItemType.HotdogSauced:
                uiInventory.makeNewMessage("Ate Okay Hotdog");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HotdogSauced, amount = 1 });
                break;
            //case Item.ItemType.Plate:
            //case Item.ItemType.PopcornEmpty:
            case Item.ItemType.PopcornFull:
                uiInventory.makeNewMessage("Ate Popcorn");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.PopcornFull, amount = 1 });
                result = inventory.AddItem(new Item { itemType = Item.ItemType.PopcornEmpty, amount = 1 });
                if (!result)
                {
                    uiInventory.makeNewMessage("Full Inventory");
                    DropItemCMD(transform.position, new Item { itemType = Item.ItemType.PopcornEmpty, amount = 1 });
                }
                break;
            case Item.ItemType.Ring:
                uiInventory.makeNewMessage("Used Ring");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Ring, amount = 1 });
                break;
            //case Item.ItemType.RubberDuck:
            case Item.ItemType.Softball:
                uiInventory.makeNewMessage("Used Softball");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Softball, amount = 1 });
                break;
            //case Item.ItemType.Soul:
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            bool result = inventory.AddItem(itemWorld.GetItem());
            if (result)
            {
                string message = "Picked Up " + itemWorld.GetItem().GetItemName();
                if (itemWorld.GetItem().amount > 1)
                {
                    message += " x" + itemWorld.GetItem().amount;
                }
                uiInventory.makeNewMessage(message);
                itemWorld.DestroySelf();
            } else {
                uiInventory.makeNewMessage("Full Inventory");
            }
        }
        SpeakerWorld speakerWorld = collider.GetComponent<SpeakerWorld>();
        if (speakerWorld != null)
        {
            moveDirection = new Vector2(0, 0);
            dialogueScript.SwitchDialogue(speakerWorld.getDialogueNumber());
        }
    }

    IEnumerator WaitForThrow()
    {
        yield return new WaitForSeconds(.2f);
        goodForThrow = true;
    }
}

//https://www.youtube.com/watch?v=u8tot-X_RBI
//https://www.youtube.com/watch?v=2WnAOV7nHW0