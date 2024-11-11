using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public Item itemHere;
    public Sprite emptyImage;
    public string typeOfItem = "all";
    public bool isLocked = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(emptyImage != null)
        {
            if (itemHere == null)
            {
                GetComponent<SpriteRenderer>().sprite = emptyImage;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = null;
            }
        }

    }
}
