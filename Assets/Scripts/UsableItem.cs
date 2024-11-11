using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : Item
{

    public Effect[] effects;


    [System.Serializable]
    public class Effect
    {
        public int bulletNumber;
        public int meleeDmg, bulletDmg;
        public float moveSpeed, bulletSpeed, bulletTime, bulletSeparation;
        public int HP, maxHP;

        public float duration;


        public Effect(int bulletNumber, int meleeDmg, int bulletDmg,
            float moveSpeed, float bulletSpeed, float bulletTime, float bulletSeparation,
            int HP, int MaxHP,
            float duration)
        {
            this.bulletNumber = bulletNumber;
            this.meleeDmg = meleeDmg;
            this.bulletDmg = bulletDmg;

            this.moveSpeed = moveSpeed;
            this.bulletSpeed = bulletSpeed;
            this.bulletTime = bulletTime;
            this.bulletSeparation = bulletSeparation;

            this.HP = HP;
            this.maxHP = MaxHP;

            this.duration = duration;
        }
    }

    public string EffectText()
    {
        string bonusTxt = "Effects:\n";

        foreach(Effect ef in effects)
        {
            if (ef.bulletNumber != 0)
            {
                if (ef.bulletNumber > 0)
                    bonusTxt += "+";

                if(ef.bulletNumber == 1 || ef.bulletNumber == -1)
                    bonusTxt +=
                        ef.bulletNumber + " bullet ";
                else
                    bonusTxt +=
                       ef.bulletNumber + " bullets ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.bulletSpeed != 0)
            {
                if (ef.bulletSpeed > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.bulletSpeed + " bullet speed ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.bulletSeparation != 0)
            {
                if (ef.bulletSeparation > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.bulletSeparation + " bullet separation ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.bulletTime != 0)
            {
                if (ef.bulletTime > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.bulletTime + " bullet time ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.bulletDmg != 0)
            {
                if (ef.bulletDmg > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.bulletDmg + " bullet dmg ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.meleeDmg != 0)
            {
                if (ef.meleeDmg > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.meleeDmg + " melee dmg ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.moveSpeed != 0)
            {
                if (ef.moveSpeed > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.moveSpeed + " speed ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.maxHP != 0)
            {
                if (ef.maxHP > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.maxHP + " max HP ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }
            if (ef.HP != 0)
            {
                if (ef.HP > 0)
                    bonusTxt += "+";

                bonusTxt +=
                   ef.HP + " HP ";

                if (ef.duration != 0)
                    bonusTxt += "for " + ef.duration + " s";
                bonusTxt += "\n";
            }



        }

        return bonusTxt;
    }

}
