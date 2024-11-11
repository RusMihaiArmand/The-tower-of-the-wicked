using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableInventorySlot : InventorySlot
{


    public IEnumerator UseItem(UsableItem.Effect ef, float duration)
    {
        PlayerMovement player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerMovement>();
        player.inventory.itemDescription.SetActive(false);

        player.baseBulletNumber += ef.bulletNumber;
        player.baseMeleeDmg += ef.meleeDmg;
        player.baseBulletDmg += ef.bulletDmg;

        player.baseMoveSpeed += ef.moveSpeed;
        player.baseBulletSpeed += ef.bulletSpeed;
        player.baseBulletTime += ef.bulletTime;
        player.baseBulletSeparation += ef.bulletSeparation;

        player.baseMaxHP += ef.maxHP;
        if(ef.maxHP > 0)
            player.SetHP(player.GetHP() + ef.maxHP);
        player.SetHP(player.GetHP() + ef.HP);
        player.StatsUpdate();

        if (duration > 0)
        {
            yield return new WaitForSeconds(duration);

            player.baseBulletNumber -= ef.bulletNumber;
            player.baseMeleeDmg -= ef.meleeDmg;
            player.baseBulletDmg -= ef.bulletDmg;

            player.baseMoveSpeed -= ef.moveSpeed;
            player.baseBulletSpeed -= ef.bulletSpeed;
            player.baseBulletTime -= ef.bulletTime;
            player.baseBulletSeparation -= ef.bulletSeparation;

            player.baseMaxHP -= ef.maxHP;
            if (ef.maxHP < 0)
                player.SetHP(player.GetHP() - ef.maxHP);
            player.SetHP(player.GetHP() - ef.HP);
            player.StatsUpdate();

        }
        else
        {
            yield return new WaitForSeconds(0f);
        }

    }

    void Use()
    {
        UsableItem item = itemHere.GetComponent<UsableItem>();

        for(int i=0;i<item.effects.Length;i++)
        {
            StartCoroutine(
                UseItem(item.effects[i],
                           item.effects[i].duration)
                );
           

        }

        Destroy(itemHere.transform.gameObject);
        itemHere = null;

    }

    void Update()
    {
        if (emptyImage != null)
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

        if(itemHere!=null)
        {
            Use(); 
        }

    }

}
