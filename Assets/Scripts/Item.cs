using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string itemName;

    [SerializeField] private Canvas canvas;
    private Transform player;
    private RectTransform rTransform;
    private CanvasGroup canvasGr;
    public bool mouseIsOver = false;

    public int bulletNumber = 0;
    public int bulletDmg = 0, meleeDmg = 0, maxHP = 0;
    public float speed = 0f, bulletTime = 0f, bulletSeparation = 0f, bulletSpeed = 0f;
    public bool dropped = true, equipped = false, inInventory = false, canBePicked = false, held = false, showing = false;
    private GameObject locatedSlot;
    public InventorySlot residence;
    public string[] canBePutIn;
    public string[] blockedSlots;
    public int worn = 0, replacementText = 0;
    public string replacementTxt; //0-normal text; 1-replacement txt; 2-normal+replacement(flavor)
    public int reverseOrder = 0;


    void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rTransform = GetComponent<RectTransform>();
        canvasGr = GetComponent<CanvasGroup>();

    }

    void Start()
    {

    }

    void Update()
    {
        if (GameStateManager.Instance.gamestate.Equals(GameState.Paused))
        {
            if(!equipped)
                OnEndDrag(new PointerEventData(EventSystem.current) );
            mouseIsOver = false;
            return;
        }

        if (Input.GetMouseButtonDown(1) && mouseIsOver)
        {
            Item g = Instantiate(Resources.Load<Item>("Prefabs/items/" + itemName),
                transform.position,
                Quaternion.identity);

            g.gameObject.transform.localScale = new Vector3(1, 1, 1);

            //g.gameObject.transform.rotation = new Vector3();
            g.gameObject.transform.position = player.transform.position;
            g.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "0";

            g.dropped = true;
            g.inInventory = false;
            g.canBePicked = false;
            g.equipped = false;

            g.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);


            g.gameObject.transform.rotation = Quaternion.Euler(g.gameObject.transform.rotation.eulerAngles.x, 
                g.gameObject.transform.rotation.eulerAngles.y,
                Random.Range(0,360));

            g.worn = this.worn;

            residence.itemHere = null;
            Destroy(this.gameObject);

            mouseIsOver = false;
            player.GetComponent<PlayerMovement>().inventory.itemDescription.SetActive(false);
            player.GetComponent<PlayerMovement>().StatsUpdate();
            player.GetComponent<PlayerMovement>().CheckCombos();
        }

        if (mouseIsOver && !dropped && !held && !equipped)
        {
            player.GetComponent<PlayerMovement>().inventory.itemDescription.SetActive(true);

            if (replacementText == 1)
            {
                player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text =
                    replacementTxt;
            }
            else
            { 

                player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text =
                    this.itemName + " - ";

                for(int pi = 0; pi < canBePutIn.Length; pi++)
                {
                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += 
                        canBePutIn[pi];
                    if (pi < canBePutIn.Length -1)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                            " ; ";

                }

                player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "\n";

                if (bulletNumber != 0)
                {
                    if (bulletNumber > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    if(bulletNumber == 1 || bulletNumber == -1)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                          bulletNumber + " bullet \n";
                    else
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                           bulletNumber + " bullets \n";
                }
                if (bulletSpeed != 0)
                {
                    if (bulletSpeed > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       bulletSpeed + " bullet speed \n";
                }
                if (bulletSeparation != 0)
                {
                    if (bulletSeparation > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       bulletSeparation + " bullet separation \n";
                }
                if (bulletTime != 0)
                {
                    if (bulletTime > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       bulletTime + " bullet time \n";
                }
                if (bulletDmg != 0)
                {
                    if (bulletDmg > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       bulletDmg + " bullet dmg \n";
                }
                if (meleeDmg != 0)
                {
                    if (meleeDmg > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       meleeDmg + " melee dmg \n";
                }
                if (speed != 0)
                {
                    if (speed > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       speed + " speed \n";
                }
                if (maxHP != 0)
                {
                    if (maxHP > 0)
                        player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text += "+";

                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       maxHP + " max HP \n";
                }

                if (this.gameObject.GetComponent<UsableItem>())
                {
                    UsableItem usabIt = this as UsableItem;
                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                       usabIt.EffectText();

                }

                if (replacementText == 2)
                    player.GetComponent<PlayerMovement>().inventory.itemDescription.GetComponentInChildren<Text>().text +=
                        replacementTxt;

            }


            


            if (!showing)
            {
                if (Input.mousePosition.x >= 295)
                    player.GetComponent<PlayerMovement>().inventory.itemDescription.transform.position =
                       new Vector3( Camera.main.transform.position.x  -4.25f,
                       player.GetComponent<PlayerMovement>().inventory.itemDescription.transform.position.y,
                       player.GetComponent<PlayerMovement>().inventory.itemDescription.transform.position.z);
                else
                    player.GetComponent<PlayerMovement>().inventory.itemDescription.transform.position =
                       new Vector3(Camera.main.transform.position.x + 4.25f,
                       player.GetComponent<PlayerMovement>().inventory.itemDescription.transform.position.y,
                       player.GetComponent<PlayerMovement>().inventory.itemDescription.transform.position.z);
            }
                

            showing = true;
        }
        else
        {
            if (showing)
            {
                showing = false;
                player.GetComponent<PlayerMovement>().inventory.itemDescription.SetActive(false);
            }
        }

        if (inInventory)
            GetComponent<Animator>().Play("In_Inventory");

        if(dropped)
            GetComponent<Animator>().Play("Dropped");

        if (dropped)
        {
            PlayerMovement player =
                GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerMovement>();

            if( Mathf.Sqrt(
           ((transform.position.x - player.transform.position.x) * (transform.position.x - player.transform.position.x)) +
           ((transform.position.y - player.transform.position.y) * (transform.position.y - player.transform.position.y)))
           >= 1.8f
           )
            {
                canBePicked = true;
            }

        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameStateManager.Instance.gamestate.Equals(GameState.Paused))
            return;

        held = true;
        if (inInventory)
        {
            canvasGr.blocksRaycasts = false;
            locatedSlot = null;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dropped)
        {
            rTransform.anchoredPosition += (eventData.delta * canvas.scaleFactor);

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        held = false;
        if(!dropped)
        {
            canvasGr.blocksRaycasts = true;

            if (locatedSlot != null &&
                    locatedSlot.GetComponent<InventorySlot>().isLocked == false)
            {
                if(locatedSlot.GetComponent<InventorySlot>().itemHere==null )
                {
                    bool canPlace = false;

                    for (int i = 0; i < canBePutIn.Length; i++)
                        if (canBePutIn[i] == locatedSlot.GetComponent<InventorySlot>().typeOfItem)
                            canPlace = true;

                    if (locatedSlot.GetComponent<InventorySlot>().typeOfItem == "all")
                        canPlace = true;

                    if (canPlace == true)
                    {
                        transform.position = locatedSlot.transform.position;
                        if (residence != null)
                            residence.itemHere = null;
                        residence = locatedSlot.GetComponent<InventorySlot>();
                        residence.itemHere = this;

                    }
                    else
                    {

                        transform.position = residence.transform.position;
                    }
                }
                else {
                    Item gottaMove = locatedSlot.GetComponent<InventorySlot>().itemHere;
                    InventorySlot tryHere = residence;

                    bool canPlace = false;

                    for (int i = 0; i < canBePutIn.Length; i++)
                        if (canBePutIn[i] == locatedSlot.GetComponent<InventorySlot>().typeOfItem)
                            canPlace = true;

                    if (locatedSlot.GetComponent<InventorySlot>().typeOfItem == "all")
                        canPlace = true;

                    if (canPlace == true)
                    {
                        transform.position = locatedSlot.transform.position;
                        if (residence != null)
                            residence.itemHere = null;
                        residence = locatedSlot.GetComponent<InventorySlot>();
                        residence.itemHere = this;

                        gottaMove.residence = null;

                        canPlace = false;

                        for (int i = 0; i < gottaMove.canBePutIn.Length; i++)
                            if (gottaMove.canBePutIn[i] == tryHere.typeOfItem)
                                canPlace = true;

                        if (tryHere.typeOfItem == "all")
                            canPlace = true;

                        if(canPlace == false)
                        {
                            for (int j = 0; j < player.GetComponent<PlayerMovement>().inventory.normalSlots.Length; j++)
                            {
                                if (player.GetComponent<PlayerMovement>().inventory.normalSlots[j].itemHere == null
                                    && canPlace == false
                                    && player.GetComponent<PlayerMovement>().inventory.normalSlots[j].isLocked == false)
                                {
                                    canPlace = true;
                                    tryHere = player.GetComponent<PlayerMovement>().inventory.normalSlots[j];
                                }

                            }
                        }

                        if(canPlace == true)
                        {
                            gottaMove.transform.position = tryHere.transform.position;
                            gottaMove.residence = tryHere.GetComponent<InventorySlot>();
                            gottaMove.transform.SetParent(gottaMove.residence.transform);
                            tryHere.GetComponent<InventorySlot>().itemHere = gottaMove;
                       
                        }
                        else
                        {

                            Item g = Instantiate(Resources.Load<Item>("Prefabs/items/" + gottaMove.itemName),
                            transform.position,Quaternion.identity);

                            g.gameObject.transform.localScale = new Vector3(1, 1, 1);

                            //g.gameObject.transform.rotation = new Vector3();
                            g.gameObject.transform.position = player.transform.position;
                            g.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "0";

                            g.dropped = true;
                            g.inInventory = false;
                            g.canBePicked = false;
                            g.equipped = false;

                            g.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);

                            Destroy(gottaMove.gameObject);



                        }


                    }
                    else
                    {
                        transform.position = residence.transform.position;
                    }


                }


            }
            else
            {
                transform.position = residence.transform.position;
            }
        }

        inInventory = false;

        for (int i = 0; i < player.GetComponent<PlayerMovement>().inventory.normalSlots.Length; i++)
            if (residence == player.GetComponent<PlayerMovement>().inventory.normalSlots[i])
                inInventory = true;

        if(residence != null)
            transform.SetParent(residence.transform);

        locatedSlot = null;

        player.GetComponent<PlayerMovement>().StatsUpdate();
        player.GetComponent<PlayerMovement>().CheckCombos();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Inventory Slot"))
        {
            locatedSlot = collision.gameObject;
        }

        if (collision.tag.Equals("Player") && dropped && canBePicked)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

            int i = 0;
            while( i < player.GetComponent<PlayerMovement>().inventory.normalSlots.Length)
            {
                if(player.GetComponent<PlayerMovement>().inventory.normalSlots[i].itemHere == null &&
                    player.GetComponent<PlayerMovement>().inventory.normalSlots[i].isLocked == false)
                {
                    GetComponent<BoxCollider2D>().size = new Vector2(2, 2); // 4,4
                    dropped = false;
                    canBePicked = false;
                    inInventory = true;
                    equipped = false;

                    residence = player.GetComponent<PlayerMovement>().inventory.normalSlots[i];
                    player.GetComponent<PlayerMovement>().inventory.normalSlots[i].itemHere = this;

                    gameObject.transform.SetParent(player.GetComponent<PlayerMovement>().inventory.transform);
                    gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "4";
                    gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
                    gameObject.transform.position = player.GetComponent<PlayerMovement>().inventory.normalSlots[i].gameObject.transform.position;
                    gameObject.transform.localScale = new Vector3(11, 11, 1);
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    i = 9 + player.GetComponent<PlayerMovement>().inventory.normalSlots.Length;
                }

                i++;
            }
            gameObject.transform.SetParent(residence.gameObject.transform);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (locatedSlot == collision.gameObject)
            locatedSlot = null; 

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
    }
}
