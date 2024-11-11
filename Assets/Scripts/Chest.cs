using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Inventory chestInventory;
    private bool playerTouch = false;
    public LootPool[] lootTable;

    [System.Serializable]
    public class LootPool
    {
        public Loot[] loot;
    }

    [System.Serializable]
    public class Loot
    {

        public Item item;
        public int chance;

        public Loot(Item item, int chance)
        {
            this.item = item;
            this.chance = chance;
        }
    }

    void Start()
    {
        for(int j=0; j< lootTable.Length; j++)
        {

            int total = 0;

            for (int i = 0; i < lootTable[j].loot.Length; i++)
                total += lootTable[j].loot[i].chance;

            int generatedChance = Random.Range(1, total + 1);

            for (int i = 0; i < lootTable[j].loot.Length; i++)
            {
                if (generatedChance <= lootTable[j].loot[i].chance && generatedChance > 0)
                {
                    if (lootTable[j].loot[i].item != null)
                    {
                        Item iii = Instantiate(lootTable[j].loot[i].item,
                         chestInventory.normalSlots[j].transform.position,
                            Quaternion.identity);

                        iii.residence = chestInventory.normalSlots[j];
                        chestInventory.normalSlots[j].itemHere = iii;

                        iii.GetComponent<BoxCollider2D>().size = new Vector2(2, 2); // 4,4
                        iii.dropped = false;
                        iii.canBePicked = false;
                        iii.inInventory = false;
                        iii.equipped = false;

                        iii.gameObject.transform.SetParent(iii.residence.transform);
                        iii.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "4";
                        iii.gameObject.transform.position = chestInventory.normalSlots[j].gameObject.transform.position;
                        iii.gameObject.transform.localScale = new Vector3(11, 11, 1);
                        iii.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

                    }


                }

                generatedChance -= lootTable[j].loot[i].chance;
            }



        }


    }

    
    void Update()
    {
        if (playerTouch == false)
            chestInventory.gameObject.SetActive(false);
        else
        {
            chestInventory.gameObject.SetActive(
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().inventory.isActiveAndEnabled
                );

          
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            playerTouch = true;

            GameObject[] slots = GameObject.FindGameObjectsWithTag("Inventory Slot");
            foreach(GameObject sl in slots)
            {
                if (sl.name.Contains("Usable"))
                    sl.GetComponent<UsableInventorySlot>().isLocked = true;
            }

            chestInventory.gameObject.transform.position = new Vector3(Camera.main.transform.position.x + 6.1f,
                    Camera.main.transform.position.y, chestInventory.gameObject.transform.position.z);

            Inventory playerInv = collision.gameObject.GetComponent<PlayerMovement>().inventory;
            playerInv.transform.position = new Vector3(playerInv.transform.position.x - 9.7f,
                playerInv.transform.position.y, playerInv.transform.position.z);



        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            playerTouch = false;

            GameObject[] slots = GameObject.FindGameObjectsWithTag("Inventory Slot");
            foreach (GameObject sl in slots)
            {
                if (sl.name.Contains("Usable"))
                    sl.GetComponent<UsableInventorySlot>().isLocked = false;
            }



            Inventory playerInv = collision.gameObject.GetComponent<PlayerMovement>().inventory;
            playerInv.transform.position = new Vector3(playerInv.transform.position.x + 9.7f,
                playerInv.transform.position.y, playerInv.transform.position.z);

            for (int i = 0; i < chestInventory.normalSlots.Length; i++)
                if(chestInventory.normalSlots[i].itemHere != null)
                    chestInventory.normalSlots[i].itemHere.transform.position =
                      chestInventory.normalSlots[i].itemHere.residence.transform.position;

            for (int i = 0; i < playerInv.normalSlots.Length; i++)
                if (playerInv.normalSlots[i].itemHere != null)
                    playerInv.normalSlots[i].itemHere.transform.position =
                      playerInv.normalSlots[i].itemHere.residence.transform.position;

        }
    }

}
