using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRedSlime : MonoBehaviour
{

    public Rigidbody2D rb;
    private Transform player;
    public int HP = 100, maxHP = 100;
    public int damage = 5;
    private bool dying = false;

    private bool oncooldown = false;
    public Projectile bullet;
    public float bulletSpeed = 4;
    public float bulletDecay = 6;

    public Animator animator;
    Vector2 movement;
    public GameObject lootChest;



    

    // Update is called once per frame
    private void Start()
    {
        animator.Play("Walk");
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }




    void Update()
    {
        if (dying)
        {
            animator.Play("Dead");
            return;
        }


        if(!oncooldown)
        {
            StartCoroutine(Attack());
        }
        
 
    }


    IEnumerator Attack()
    {
        oncooldown = true;

        int atN = Random.Range(0, 2);

        if(atN == 0)
        {

            for (int bn = 1; bn <= 10; bn++)
                if (!dying)
                {
                    Projectile projectile =
                        Instantiate(bullet, transform.position +
                        new Vector3(0, -2 , 0),
                        Quaternion.identity);

                    projectile.targetX = player.position.x;
                    projectile.targetY = player.position.y;
                    projectile.speed = bulletSpeed;
                    projectile.lifeTime = bulletDecay;

                    yield return new WaitForSeconds(0.2f);
                }
            

        }
        else
        {
            Instantiate(Resources.Load<EnemySlime>("Prefabs/enemies/Slime"), new Vector3(transform.position.x, transform.position.y - 3.5f, 1), Quaternion.identity);
        }

        yield return new WaitForSeconds( Random.Range(70, 100)/10  );
        oncooldown = false;

    }



    IEnumerator Die()
    {
        dying = true;
        movement.x = 0;
        movement.y = 0;
        Destroy(this.GetComponent<BoxCollider2D>());
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        animator.Play("Dead");
        Instantiate(lootChest, new Vector3(transform.position.x, transform.position.y - 2.5f, 1), Quaternion.identity);

        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }



    void FixedUpdate()
    {
        if(dying)
            return;


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dying)
            return;
        if (collision.tag.Equals("Player Projectile"))
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();

            HP -= proj.dmg;
            
            if (HP <= 0)
                HP = 0;

            if (HP == 0)
            {
                StartCoroutine(Die());
            }
        }


        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dying)
            return;
        if (collision.collider.tag.Equals("Player"))
        {
            player.GetComponent<PlayerMovement>().TakenDamage(this.damage);
        }
    }

}
