using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] normalSlots;
    public InventorySlot[] equipmentSlots;
    public UsableInventorySlot usableSlot;
    public GameObject itemDescription;
    
    void Start()
    {
       
    }

    void Update()
    {
        

        for (int i = 0; i < equipmentSlots.Length; i++)
            equipmentSlots[i].isLocked = false;

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].itemHere != null)
            {
                for(int j = 0; j < equipmentSlots[i].itemHere.blockedSlots.Length; j ++)
                {
                    for (int y = 0; y < equipmentSlots.Length; y++)
                        if (equipmentSlots[y].typeOfItem == equipmentSlots[i].itemHere.blockedSlots[j])
                        {
                            equipmentSlots[y].isLocked = true;
                            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().StatsUpdate();
                        }

                }

            }
        }


        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].isLocked == true && equipmentSlots[i].itemHere != null)
            {
                bool canPlace = false;
                InventorySlot tryHere = null;
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                for (int j = 0; j < normalSlots.Length; j++)
                {
                    if (normalSlots[j].itemHere == null && canPlace == false
                        && normalSlots[j].isLocked == false)
                    {
                        canPlace = true;
                        tryHere = normalSlots[j];
                    }

                }


                if (canPlace == true)
                {
                    tryHere.itemHere = equipmentSlots[i].itemHere;
                    equipmentSlots[i].itemHere.transform.position = tryHere.transform.position;
                    equipmentSlots[i].itemHere.residence = tryHere;
                    equipmentSlots[i].itemHere = null;
                }
                else
                {

                    Item g = Instantiate(Resources.Load<Item>("Prefabs/items/" + 
                         equipmentSlots[i].itemHere.itemName),
                    transform.position, Quaternion.identity);

                    g.gameObject.transform.localScale = new Vector3(1, 1, 1);

                    g.gameObject.transform.position = player.transform.position;
                    g.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "0";

                    g.dropped = true;
                    g.inInventory = false;
                    g.canBePicked = false;
                    g.equipped = false;

                    g.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);

                    Destroy(equipmentSlots[i].itemHere.gameObject);

                    equipmentSlots[i].itemHere = null;

                }


            }

        }
            


    }
}
