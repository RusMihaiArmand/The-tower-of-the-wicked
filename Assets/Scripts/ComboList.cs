using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboList : MonoBehaviour
{
    public PlayerMovement player;

    public Combo[] combos;


    [System.Serializable]
    public class Combo
    {
        public int bulletNumber;
        public int meleeDmg, bulletDmg;
        public float moveSpeed, bulletSpeed, bulletTime, bulletSeparation;
        public int maxHP;

        public string comboName;
        public bool active = false, unlocked = false;
        public GameObject comboBlocker;

        public EquipmentList[] requiredEquipment;


        [System.Serializable]
        public class EquipmentList
        {
            public bool eChecked = false;
            public string[] eNames;

        }

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HardReset()
    {
        for(int i=0; i<combos.Length; i++)
        {
            if(combos[i].active == true)
            {
                player.baseBulletNumber -= combos[i].bulletNumber;
                player.baseMeleeDmg -= combos[i].meleeDmg;
                player.baseBulletDmg -= combos[i].bulletDmg;

                player.baseMoveSpeed -= combos[i].moveSpeed;
                player.baseBulletSpeed -= combos[i].bulletSpeed;
                player.baseBulletTime -= combos[i].bulletTime;
                player.baseBulletSeparation -= combos[i].bulletSeparation;

                player.baseMaxHP -= combos[i].maxHP;
                if (combos[i].maxHP < 0)
                    player.SetHP(player.GetHP() - combos[i].maxHP);

                player.StatsUpdate();
            }
            combos[i].active = false;

            for (int j = 0; j < combos[i].requiredEquipment.Length; j++)
            {
                combos[i].requiredEquipment[j].eChecked = false;
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
