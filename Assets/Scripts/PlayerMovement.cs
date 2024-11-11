using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public float baseMoveSpeed = 5f;
    private float moveSpeed = 0f;
    public int baseMaxHP = 10;
    private int HP = 0, maxHP = 0;
    public Rigidbody2D rb;
    public Animator animator;
    public int facing = 0;  // 0-D  1-L  2-U  3-R        LR priority over UD
    private int going = 0; /// when going between rooms; 1-L, 2-U, 3-R, 4-D ;   0-not moving between rooms
    private string currentAnimation = "Player_Idle_Up";
    private bool attacking = false, attacked = false;
    public Sprite meleeHitSprite, rangedHitSprite;
    public Projectile meleeHit;
    private bool continuousAttack = false;
    private string attackDirection = "-";
    public int baseBulletNumber = 4;
    private int bulletNumber = 0;
    public float baseBulletTime = 2f, baseBulletSeparation = 20f, baseBulletSpeed = 3.2f;
    private float bulletRange = 6f, bulletTime = 0f, bulletSeparation = 0f, bulletSpeed = 0f; ///range doesn't seem to be useful; keep it so it doesn't break anything pls. signed, past me
    public int baseBulletDmg = 2, baseMeleeDmg = 5;
    private int bulletDmg = 2, meleeDmg = 5;
    public int monstersToKill = 0;
    Vector2 movement;
    public Item[] equipment;
    public Inventory inventory;
    public SpriteRenderer HPbar;
    public Text HPtext;
    public Map map;
    public ComboList listOfCombos;
    private bool invincibility = false;
    private float invicibilityTime = 3f;


    void Start()
    {
        HP = baseMaxHP;
        StatsUpdate();
    }

    void Update()
    {
        if (GameStateManager.Instance.gamestate.Equals(GameState.Paused))
            return;



        HPtext.text = HP + " / " + maxHP;

        string HPsprite = "HP/";

        if (HP == maxHP)
            HPsprite += "full HP";
        else
        {
            if(HP >= 8 * maxHP / 10)
                HPsprite += "injured 1";
            else
            {
                if (HP >= 6 * maxHP / 10)
                    HPsprite += "injured 2";
                else
                {
                    if (HP >= 4 * maxHP / 10)
                        HPsprite += "injured 3";
                    else
                    {
                        if (HP >= 2 * maxHP / 10)
                            HPsprite += "injured 4";
                        else
                        {
                            if (HP > 0)
                                HPsprite += "injured 5";
                            else
                            {
                                HPsprite += "dead";
                                Time.timeScale = 0;
                            }


                        }
                    }
                }
            }

        }


        HPbar.sprite = Resources.Load<Sprite>(HPsprite);

        if (Input.GetButtonDown("Inventory"))
        {
            if (inventory.gameObject.activeSelf) //!!!
            {
                inventory.gameObject.SetActive(false);
                

                foreach(InventorySlot invs in inventory.equipmentSlots)
                {
                    if (invs.itemHere != null)
                        invs.itemHere.mouseIsOver = false;
                }

                foreach (InventorySlot invs in inventory.normalSlots)
                {
                    if (invs.itemHere != null)
                        invs.itemHere.mouseIsOver = false;
                }

                inventory.itemDescription.SetActive(false);
            }
            else
            {
                inventory.gameObject.SetActive(true);
                inventory.itemDescription.SetActive(false);

                for (int i = 0; i < inventory.equipmentSlots.Length; i++)
                    if (inventory.equipmentSlots[i].itemHere != null)
                        inventory.equipmentSlots[i].itemHere.transform.position =
                            inventory.equipmentSlots[i].itemHere.residence.transform.position;

                for (int i = 0; i < inventory.normalSlots.Length; i++)
                    if (inventory.normalSlots[i].itemHere != null)
                        inventory.normalSlots[i].itemHere.transform.position =
                            inventory.normalSlots[i].itemHere.residence.transform.position;

            }
        }

        if (Input.GetButtonDown("Map"))
        {
            for(int i = 0; i<map.mapParts.Length; i ++)
            {
                if (map.mapParts[i].activeSelf)  ///!!!
                    map.mapParts[i].SetActive(false);
                else
                    map.mapParts[i].SetActive(true);

            }

        }


        //bulletNumber = baseBulletNumber;
        //for (int eq = 0; eq < equipment.Length; eq++)
        //{
        //    if (inventory.equipmentSlots[eq].itemHere != null && equipment[eq] == null)
        //    {
        //        equipment[eq] = Instantiate(Resources.Load<Item>("Prefabs/items/" + inventory.equipmentSlots[eq].itemHere.itemName),
        //        transform.position,
        //        Quaternion.identity);

        //        equipment[eq].gameObject.transform.localScale = transform.localScale;

        //        equipment[eq].gameObject.transform.SetParent(transform);

        //        equipment[eq].gameObject.GetComponent<Item>().inInventory = false;
        //        equipment[eq].gameObject.GetComponent<Item>().equipped = true;
        //        equipment[eq].gameObject.GetComponent<Item>().dropped = false;
        //        equipment[eq].gameObject.GetComponent<Item>().canBePicked = false;

        //        equipment[eq].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "3";

        //        Destroy(equipment[eq].gameObject.GetComponent<Rigidbody2D>());
                
        //    }

        //    if (inventory.equipmentSlots[eq].itemHere == null && equipment[eq] != null)
        //    {
        //        Destroy(equipment[eq].gameObject);
        //        equipment[eq] = null;

        //    }

        //    if (inventory.equipmentSlots[eq].itemHere != null && equipment[eq] != null)
        //        if (inventory.equipmentSlots[eq].itemHere.itemName != equipment[eq].itemName)
        //        {
        //            Destroy(equipment[eq].gameObject);
        //            equipment[eq] = null;

        //            equipment[eq] = Instantiate(Resources.Load<Item>("Prefabs/items/" + inventory.equipmentSlots[eq].itemHere.itemName),
        //            transform.position,
        //            Quaternion.identity);

        //            equipment[eq].gameObject.transform.localScale = transform.localScale;

        //            equipment[eq].gameObject.transform.SetParent(transform);

        //            equipment[eq].gameObject.GetComponent<Item>().inInventory = false;
        //            equipment[eq].gameObject.GetComponent<Item>().equipped = true;
        //            equipment[eq].gameObject.GetComponent<Item>().dropped = false;
        //            equipment[eq].gameObject.GetComponent<Item>().canBePicked = false;

        //            equipment[eq].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "3";

        //            Destroy(equipment[eq].gameObject.GetComponent<Rigidbody2D>());

        //        }


        //    if (equipment[eq] != null)
        //    {

        //        bulletNumber += equipment[eq].bulletNumber;

        //    }
        //}
            



        int numberOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Count();

        if(numberOfEnemies != monstersToKill)
        {
            if(monstersToKill == 0)
            {
                ///monsters spawned; LOCK doors
                

                GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

                foreach (GameObject aDoor in doors)
                {

                    if (
                        (((aDoor.transform.position.x == Camera.main.transform.position.x + 8.5f) ||
                        (aDoor.transform.position.x == Camera.main.transform.position.x - 8.5f)) &&
                        ((aDoor.transform.position.y == Camera.main.transform.position.y + 0.5f) ||
                        (aDoor.transform.position.y == Camera.main.transform.position.y - 0.5f)))
                        ||
                        (((aDoor.transform.position.x == Camera.main.transform.position.x + 0.5f) ||
                        (aDoor.transform.position.x == Camera.main.transform.position.x - 0.5f)) &&
                        ((aDoor.transform.position.y == Camera.main.transform.position.y + 4.5f) ||
                        (aDoor.transform.position.y == Camera.main.transform.position.y - 4.5f)))
                        )
                    {
                        GameObject closedDoor = Resources.Load<GameObject>("Prefabs/tiles/Closed Doors/" +
                            aDoor.GetComponent<SpriteRenderer>().sprite.name + " Closed");

                        Instantiate(closedDoor,
                            new Vector3(aDoor.transform.position.x, aDoor.transform.position.y, 0.5f), Quaternion.identity);
                    }

                }

            }
            else
            {
                if(numberOfEnemies == 0)
                {
                    ///monsters eliminated; UNLOCK doors
                   

                    GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

                    foreach (GameObject aDoor in doors)
                    {

                        if (aDoor.GetComponent<SpriteRenderer>().sprite.name.Contains("Closed"))
                        {
                            Destroy(aDoor);
                        }

                    }


                }

            }

            monstersToKill = numberOfEnemies;
        }

   

        string animationToPlay = "-";

 

        if (this.gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("4")  && attacking && !attacked)
        {
            attacked = true;

            float toX = 0, toY = 0;

            if (currentAnimation.Contains("Attack_Down"))
                toY = -1.35f;
            if (currentAnimation.Contains("Attack_Up"))
                toY = 1.35f;
            if (currentAnimation.Contains("Attack_Left"))
                toX = -0.8f;
            if (currentAnimation.Contains("Attack_Right"))
                toX = 0.8f;


            ///GET OUT OF COMMENT

            Projectile meleeProjectile = Instantiate(meleeHit,
           new Vector3(transform.position.x + toX, transform.position.y + toY),
           Quaternion.identity);

            meleeProjectile.targetX = transform.position.x;
            meleeProjectile.targetY = transform.position.y - 0.9f;
            meleeProjectile.speed = 0;
            meleeProjectile.lifeTime = 0.2f;
            meleeProjectile.dmg = meleeDmg;

            meleeProjectile.gameObject.GetComponent<SpriteRenderer>().sprite = meleeHitSprite;

            if (currentAnimation.Contains("Attack_Right"))
                meleeProjectile.transform.rotation = Quaternion.Euler(meleeProjectile.transform.rotation.x, meleeProjectile.transform.rotation.y, 90);

            if (currentAnimation.Contains("Attack_Up"))
                meleeProjectile.transform.rotation = Quaternion.Euler(meleeProjectile.transform.rotation.x, meleeProjectile.transform.rotation.y, 180);

            if (currentAnimation.Contains("Attack_Left"))
                meleeProjectile.transform.rotation = Quaternion.Euler(meleeProjectile.transform.rotation.x, meleeProjectile.transform.rotation.y, 270);

            meleeProjectile.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;

            Projectile[] rangedProjectile = new Projectile[bulletNumber];



            int degrees = 0;


            if (currentAnimation.Contains("Attack_Left"))
                degrees = 180;
            if (currentAnimation.Contains("Attack_Up"))
                degrees = 90;
            if (currentAnimation.Contains("Attack_Down"))
                degrees = 270;

            for (int count = 1; count <= bulletNumber; count++)
            {
                rangedProjectile[count - 1] = Instantiate(meleeHit,
                new Vector3(transform.position.x + toX, transform.position.y + toY),
                Quaternion.identity);

                rangedProjectile[count - 1].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                rangedProjectile[count - 1].gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0083f, 0.003f);
                rangedProjectile[count - 1].gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.4315f, 0.4593f);

                float degreeModifier = bulletSeparation;

                if (count == bulletNumber && ((bulletNumber % 2) == 1))
                {
                   
                    degreeModifier = 0;
                }
                else
                {
                    if (count % 2 == 0)
                    {
                        degreeModifier = degreeModifier * (count / 2);

                        if (bulletNumber % 2 == 0)
                            degreeModifier -= bulletSeparation / 2;
                    }
                    else
                    {
                        degreeModifier = -1 * degreeModifier * (count+1) / 2;
                        if (bulletNumber % 2 == 0)
                            degreeModifier += bulletSeparation / 2;
                    }
                }


                Vector2 position = new Vector2( Mathf.Cos( (degrees + degreeModifier) * Mathf.Deg2Rad)  ,
                    Mathf.Sin((degrees + degreeModifier) * Mathf.Deg2Rad)) 
                    * bulletRange;

                rangedProjectile[count - 1].targetX = transform.position.x + toX + position.x;
                rangedProjectile[count - 1].targetY = transform.position.y + toY + position.y;
                rangedProjectile[count - 1].speed = bulletSpeed;
                rangedProjectile[count - 1].lifeTime = bulletTime;
                rangedProjectile[count - 1].dmg = bulletDmg;

                if (currentAnimation.Contains("Attack_Right"))
                    rangedProjectile[count - 1].targetX += bulletRange;
                if (currentAnimation.Contains("Attack_Left"))
                    rangedProjectile[count - 1].targetX -= bulletRange;
                if (currentAnimation.Contains("Attack_Up"))
                    rangedProjectile[count - 1].targetY += bulletRange;
                if (currentAnimation.Contains("Attack_Down"))
                    rangedProjectile[count - 1].targetY -= bulletRange;

                rangedProjectile[count - 1].gameObject.GetComponent<SpriteRenderer>().sprite = rangedHitSprite;

            }


            /// --------------------------- ADDING NEW EQUIPMENT ON PLAYER CHARACTER

            //GameObject thing = new GameObject();
            //thing.transform.SetParent(transform);
            //thing.transform.position = transform.position;
            //thing.AddComponent(typeof(SpriteRenderer));
            //thing.GetComponent<SpriteRenderer>().sortingLayerName = "3";

            //thing.AddComponent(typeof(Animator));
            //thing.GetComponent<Animator>().runtimeAnimatorController =
            //    Resources.Load<RuntimeAnimatorController>("Red Pants");

            //thing.transform.localScale = new Vector3(1, 1, 1);

            //equipment[1] = thing;

            /// ---------------------------


        }


        if (Input.GetButtonDown("Shoot Down"))
        {
            attackDirection = "Down";
            continuousAttack = true;
        }
        if(Input.GetButtonUp("Shoot Down") && (attackDirection == "Down"))
        {
            continuousAttack = false;
        }


        if (Input.GetButtonDown("Shoot Left"))
        {
            attackDirection = "Left";
            continuousAttack = true;
        }
        if (Input.GetButtonUp("Shoot Left") && attackDirection == "Left")
        {
            continuousAttack = false;
        }


        if (Input.GetButtonDown("Shoot Up"))
        {
            attackDirection = "Up";
            continuousAttack = true;
        }
        if (Input.GetButtonUp("Shoot Up") && attackDirection == "Up")
        {
            continuousAttack = false;
        }


        if (Input.GetButtonDown("Shoot Right"))
        {
            attackDirection = "Right";
            continuousAttack = true;
        }
        if (Input.GetButtonUp("Shoot Right") && attackDirection == "Right")
        {
            continuousAttack = false;
        }


        if (attackDirection != "-")
        {
           /// continuousAttack = true;
            //LeHit = Resources.Load<Sprite>("hits/hit_down");
            string meleeHitName = "hits/", rangedHitName = "hits/";


            //if(arma)
            //{
            //    name += "weapons/";
            //    name += arma.nume;
            //}


            //if(!arma && manusi)
            //{
            //    name += "gloves/";
            //    name += gloves.nume;
            //}


            //if(!arma && !manusi)
            meleeHitName += "hit punch";
            rangedHitName += "default bullet";


            meleeHitSprite = Resources.Load<Sprite>(meleeHitName);
            rangedHitSprite = Resources.Load<Sprite>(rangedHitName);

        }


        if (!attacking && !continuousAttack)
        {
            attackDirection = "-";
            if (animationToPlay == "-")
            { 
                if (movement.sqrMagnitude < 0.01) // has like no speed, so stay put
                {
                    if (facing == 0)
                        animationToPlay = "Player_Idle_Down";
                    if (facing == 1)
                        animationToPlay = "Player_Idle_Left";
                    if (facing == 2)
                        animationToPlay = "Player_Idle_Up";
                    if (facing == 3)
                        animationToPlay = "Player_Idle_Right";
                }
                else
                {
                    if (movement.x != 0)
                    {
                        if (movement.x < 0)
                            animationToPlay = "Player_Walk_Left";
                        else
                            animationToPlay = "Player_Walk_Right";
                    }
                    else
                    {
                        if (movement.y < 0)
                            animationToPlay = "Player_Walk_Down";
                        else
                            animationToPlay = "Player_Walk_Up";
                    }
                }

            }

        
            if(animationToPlay != currentAnimation)
            {
                currentAnimation = animationToPlay;
                animator.Play(animationToPlay);
                for (int eq = 0; eq < equipment.Length; eq++)
                    if (equipment[eq] != null)
                    {
                        equipment[eq].GetComponent<Animator>().Play(animationToPlay);
                        if(equipment[eq].reverseOrder ==1)
                        {
                            if (currentAnimation.Contains("Up"))
                                equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                                    -Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                            else
                                equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                                    Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                        }

                        if (equipment[eq].reverseOrder == 2)
                        {
                            if (currentAnimation.Contains("Down"))
                                equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                                    -Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                            else
                                equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                                    Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                        }


                    }

            }


        }
        else
        {

            if (!attacking)
            {



                if (movement.y == 0)
                {
                    if(movement.x ==0)
                        animationToPlay = "Player_Attack_" + attackDirection +"_Still";
                    else
                    {
                        if (movement.x > 0)
                            animationToPlay = "Player_Attack_" + attackDirection + "_Going_Right";
                        else
                            animationToPlay = "Player_Attack_" + attackDirection + "_Going_Left";

                    }
                }
                else
                {
                    
                    if (movement.y > 0)
                        animationToPlay = "Player_Attack_" + attackDirection + "_Going_Up";
                    else
                        animationToPlay = "Player_Attack_" + attackDirection + "_Going_Down";
                }

                attacking = true;
                attacked = false;

                if (animationToPlay != currentAnimation)
                {
                    currentAnimation = animationToPlay;
                    animator.Play(animationToPlay);
                    for (int eq = 0; eq < equipment.Length; eq++)
                        if (equipment[eq] != null)
                            equipment[eq].GetComponent<Animator>().Play(animationToPlay);
                }

                Invoke("DoneAttacking", 0.48f); //0.5f

            }

        }
      

        if (going != -1)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if(
                !this.gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("6") &&
                !this.gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("5")
                )
            { 
                if (currentAnimation.Contains("Still"))
                {
                    movement.x = 0;
                    movement.y = 0;
                }

                if (currentAnimation.Contains("Going_Down"))
                    movement.y = -1;

                if (currentAnimation.Contains("Going_Up"))
                    movement.y = 1;

                if (currentAnimation.Contains("Going_Left"))
                    movement.x = -1;

                if (currentAnimation.Contains("Going_Right"))
                    movement.x = 1;

            }

        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }



        if (movement.y > 0)
            facing = 2;
        if (movement.y < 0)
            facing = 0;

        if (movement.x > 0)
            facing = 3;
        if (movement.x < 0)
            facing = 1;

        if (going != 0 && going != -1)
        {
 
                rb.bodyType = RigidbodyType2D.Kinematic;
                StartCoroutine(MovingBetweenRooms());
  
        }

    }

    public void StatsUpdate()
    {

        bulletNumber = baseBulletNumber;
        moveSpeed = baseMoveSpeed;
        maxHP = baseMaxHP;

        bulletSeparation = baseBulletSeparation;
        bulletTime = baseBulletTime;
        bulletSpeed = baseBulletSpeed;

        bulletDmg = baseBulletDmg;
        meleeDmg = baseMeleeDmg;

        //moveSpeed = baseMoveSpeed;
        for (int eq = 0; eq < equipment.Length; eq++)
        {
            if (inventory.equipmentSlots[eq].itemHere != null && equipment[eq] == null)
            {
                equipment[eq] = Instantiate(Resources.Load<Item>("Prefabs/items/" + inventory.equipmentSlots[eq].itemHere.itemName),
                transform.position,
                Quaternion.identity);

                equipment[eq].gameObject.transform.localScale = transform.localScale;

                equipment[eq].gameObject.transform.SetParent(transform);

                equipment[eq].gameObject.GetComponent<Item>().inInventory = false;
                equipment[eq].gameObject.GetComponent<Item>().equipped = true;
                equipment[eq].gameObject.GetComponent<Item>().dropped = false;
                equipment[eq].gameObject.GetComponent<Item>().canBePicked = false;

                ///equipment[eq].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "3";

                Destroy(equipment[eq].gameObject.GetComponent<Rigidbody2D>());

            }

            if (inventory.equipmentSlots[eq].itemHere == null && equipment[eq] != null)
            {
                Destroy(equipment[eq].gameObject);
                equipment[eq] = null;

            }

            if (inventory.equipmentSlots[eq].itemHere != null && equipment[eq] != null)
                if (inventory.equipmentSlots[eq].itemHere.itemName != equipment[eq].itemName)
                {
                    Destroy(equipment[eq].gameObject);
                    equipment[eq] = null;

                    equipment[eq] = Instantiate(Resources.Load<Item>("Prefabs/items/" + inventory.equipmentSlots[eq].itemHere.itemName),
                    transform.position,
                    Quaternion.identity);

                    equipment[eq].gameObject.transform.localScale = transform.localScale;

                    equipment[eq].gameObject.transform.SetParent(transform);

                    equipment[eq].gameObject.GetComponent<Item>().inInventory = false;
                    equipment[eq].gameObject.GetComponent<Item>().equipped = true;
                    equipment[eq].gameObject.GetComponent<Item>().dropped = false;
                    equipment[eq].gameObject.GetComponent<Item>().canBePicked = false;

                    ///equipment[eq].gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "3";

                    Destroy(equipment[eq].gameObject.GetComponent<Rigidbody2D>());

                }


            if (equipment[eq] != null)
            {
                maxHP += equipment[eq].maxHP;
                if(inventory.equipmentSlots[eq].itemHere.worn < 1)
                {
                    inventory.equipmentSlots[eq].itemHere.worn = 1;
                    HP += equipment[eq].maxHP;
                }

                moveSpeed += equipment[eq].speed;

                bulletNumber += equipment[eq].bulletNumber;
                bulletSpeed += equipment[eq].bulletSpeed;
                bulletSeparation += equipment[eq].bulletSeparation;
                bulletTime += equipment[eq].bulletTime;
                bulletDmg += equipment[eq].bulletDmg;

                meleeDmg += equipment[eq].meleeDmg;

            }
        }

        if (HP > maxHP)
            HP = maxHP;
        if (bulletNumber < 0)
            bulletNumber = 0;
        if (moveSpeed < 0.5f)
            moveSpeed = 0.5f;
        if (bulletSeparation < 0)
            bulletSeparation = 0;
        if (bulletTime < 0.1f)
            bulletTime = 0.1f;
        if (bulletSpeed < 0.1f)
            bulletSpeed = 0.1f;
        if (bulletDmg < 0)
            bulletDmg = 0;
        if (meleeDmg < 0)
            meleeDmg = 0;



        animator.Play(currentAnimation,-1,0);
        for (int eq = 0; eq < equipment.Length; eq++)
            if (equipment[eq] != null)
            {
                ///check/redo this whole thing
                equipment[eq].GetComponent<Animator>().Play(currentAnimation, -1, 0);
                if (equipment[eq].reverseOrder == 1)
                {
                    if (currentAnimation.Contains("Up"))
                        equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                            -Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                    else
                        equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                            Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                }

                if (equipment[eq].reverseOrder == 2)
                {
                    if (currentAnimation.Contains("Down"))
                        equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                            -Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                    else
                        equipment[eq].GetComponent<SpriteRenderer>().sortingOrder =
                            Mathf.Abs(equipment[eq].GetComponent<SpriteRenderer>().sortingOrder);
                }


            }


    }

    public void SetHP(int H)
    {
        HP = H;
    }

    public int GetHP()
    {
        return HP;
    }

    void DoneAttacking()
    {
        attacking = false;
    }

    IEnumerator MovingBetweenRooms()
    {
        /// when going between rooms; 1-L, 2-U, 3-R, 4-D ; 
        float movingX = 0, movingY = 0;


 
        if (going == 1)
        {
            movingX = -1.0f;
            facing = 1;
        }
        if (going == 3)
        {
            movingX = 1.0f;
            facing = 3;
        }
        if (going == 2)
        {
            movingY = 1.0f;
            facing = 2;
        }
        if (going == 4)
        {
            movingY = -1.0f;
            facing = 0;
        }


        going = -1;

        if (movingX == 0)
            transform.position = new Vector2(Camera.main.transform.position.x, transform.position.y);
        if (movingY == 0)
            transform.position = new Vector2(transform.position.x, Camera.main.transform.position.y);

        float initialCamX = Camera.main.transform.position.x;
        float initialCamY = Camera.main.transform.position.y;


        for (int i = 1; i <= 20; i++)
        {

            //up-down = 3.3                10 for rooms
            //left-right = 3.7             18 for rooms

            transform.position = new Vector3(transform.position.x + movingX * 0.148f, transform.position.y + movingY * 0.165f, transform.position.z);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + movingX * 0.9f, Camera.main.transform.position.y + movingY * 0.5f, Camera.main.transform.position.z);
            yield return new WaitForSeconds(0.01f);

            if (i == 20)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                going = 0;

                

            }
        }

        if (movingX == 1)
            initialCamX += 18;
        if (movingX == -1)
            initialCamX -= 18;
        if (movingY == 1)
            initialCamY += 10;
        if (movingY == -1)
            initialCamY -= 10;

        Camera.main.transform.position = 
            new Vector3(initialCamX, initialCamY, Camera.main.transform.position.z);

        map.UpdateMap();

    }

    IEnumerator InvincibilityCooldown()
    {
        invincibility = true;
        yield return new WaitForSeconds(invicibilityTime);
        invincibility = false;
    }


    void FixedUpdate()
    {
        

        float slower = 1;
        if (attacking)
            slower = 0.5f;
        ///rb.MovePosition(rb.position + slower * movement * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + slower * movement * moveSpeed * Time.fixedDeltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (going == 0)
        {
            string name = collision.collider.name;

            //alea cu 1: hitbox jos, pt circulatie;  alea cu 2: hitbox pe margini, pt protectie
            if(!name.Contains("Closed") && monstersToKill==0)
            {
                if (name.Contains("1 Left"))
                    going = 1;
                if (name.Contains("1 Up"))
                    going = 2;
                if (name.Contains("1 Right"))
                    going = 3;
                if (name.Contains("1 Down"))
                    going = 4;
            }
            
        }

        if(collision.collider.tag.Equals("Projectile"))
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();
            this.TakenDamage(proj.dmg);

            //if (!invincibility)
            //{
            //    StartCoroutine(InvincibilityCooldown());
            //    //Debug.Log("HIT; LOST " + proj.dmg + " HP ");
            //    HP -= proj.dmg;

            //    if (HP <= 0)
            //        HP = 0;

            //    if (HP == 0)
            //    {
            //        //Debug.Log("lost lol");
            //        ///DO SOMETHING TO LOSE

            //    }

                
            //}

        }


        //up-down = 3.6                10 for rooms
        //left-right = 3.7             18 for rooms
    }


    public void TakenDamage(int dmgTaken)
    {

        if (!invincibility)
        {
            StartCoroutine(InvincibilityCooldown());
            //Debug.Log("HIT; LOST " + proj.dmg + " HP ");
            HP -= dmgTaken;

            if (HP <= 0)
                HP = 0;

            if (HP == 0)
            {
                //Debug.Log("lost lol");
                ///DO SOMETHING TO LOSE

            }


        }

    }

    public float GetStats(string data)
    {
        if (data == "MaxHP")
            return maxHP;

        if (data == "Speed")
            return moveSpeed;




        if (data == "BulletNumber")
            return bulletNumber;

        if (data == "BulletSpeed")
            return bulletSpeed;

        if (data == "BulletTime")
            return bulletTime;

        if (data == "BulletSeparation")
            return bulletSeparation;

        if (data == "BulletDmg")
            return bulletDmg;

        if (data == "MeleeDmg")
            return meleeDmg;

        return 0;
    }

    public void CheckCombos()
    {
        listOfCombos.HardReset();
        int i = 0, j =0;
        while ( i < equipment.Length)
        {
            if(equipment[i] != null)
            {
                j = 0;
                ///look through all combos
                while (j < listOfCombos.combos.Length)
                {
                    bool used = false;
                    if (listOfCombos.combos[j].active == false)
                    {
                        int y = 0;
                        while (y < listOfCombos.combos[j].requiredEquipment.Length && used == false)
                        {
                            if (listOfCombos.combos[j].requiredEquipment[y].eChecked == false)
                            {
                                int yz = 0;
                                while (yz < listOfCombos.combos[j].requiredEquipment[y].eNames.Length && used == false)
                                {
                                    if (listOfCombos.combos[j].requiredEquipment[y].eNames[yz] == equipment[i].itemName)
                                    {
                                        listOfCombos.combos[j].requiredEquipment[y].eChecked = true;
                                        used = true;
                                    }
                                    yz++;
                                }
                                y++;
                            }
                            else
                                y++;
                        }
                    }
                    j++;
                }
            }
            i++;
        }


        j = 0;
        ///look through all combos
        while (j < listOfCombos.combos.Length)
        {
            bool couldActivate = true;
            int y = 0;
            while (y < listOfCombos.combos[j].requiredEquipment.Length)
            {
                if (listOfCombos.combos[j].requiredEquipment[y].eChecked == false)
                {
                    couldActivate = false;
                    y = listOfCombos.combos[j].requiredEquipment.Length + 2;
                }
                else
                    y++;
            }

            if(couldActivate == true)
            {
                listOfCombos.combos[j].active = true;
                listOfCombos.combos[j].comboBlocker.SetActive(false);
                listOfCombos.combos[j].unlocked = true;
            }

            j++;
        }


        for ( i = 0; i < listOfCombos.combos.Length; i++)
        {
            if (listOfCombos.combos[i].active == true)
            {
                baseBulletNumber += listOfCombos.combos[i].bulletNumber;
                baseMeleeDmg += listOfCombos.combos[i].meleeDmg;
                baseBulletDmg += listOfCombos.combos[i].bulletDmg;

                baseMoveSpeed += listOfCombos.combos[i].moveSpeed;
                baseBulletSpeed += listOfCombos.combos[i].bulletSpeed;
                baseBulletTime += listOfCombos.combos[i].bulletTime;
                baseBulletSeparation += listOfCombos.combos[i].bulletSeparation;

                baseMaxHP += listOfCombos.combos[i].maxHP;
                if(listOfCombos.combos[i].maxHP >0)
                    SetHP(GetHP() + listOfCombos.combos[i].maxHP);

                StatsUpdate();
            }

        }

    }

}
