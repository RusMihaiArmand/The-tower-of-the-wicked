using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEyeball : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float keepDistance = 3f;
    public float followDistance = 5f;
    private bool reChoose = true;
    private float chooseTimer = 0;
    public Rigidbody2D rb;
    private int blockagesUp = 0, blockagesDown = 0, blockagesLeft = 0, blockagesRight = 0;
    private Transform player;
    public Projectile bullet;
    public float bulletSpeed = 1;
    public float bulletDecay = 9;
    public float fireDelay = 3f;
    private bool canFire = true;
    public int HP = 2, maxHP = 2;
    // public Animator animator;

    Vector2 movement;
    private List<PathNode> correctPath;
    public bool found;
    private Grid grid;

    // Update is called once per frame
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        grid = new Grid(18 , 10 , 0.5f, Camera.main.transform.position - new Vector3(9, 5, 0));
        found = false;
        

    }
    void Update()
    {
        
        ///for moving
        if (reChoose == true)
        { 
        movement.x = 0;
        movement.y = 0;

        chooseTimer = 0;
        /*

        if (keepDistance >  Mathf.Sqrt( 
           (  (rb.position.x - player.transform.position.x ) * (rb.position.x - player.transform.position.x))  +
           ((rb.position.y - player.transform.position.y) * (rb.position.y - player.transform.position.y))
           )
            )
        {
            bool canGoUp = true, canGoDown = true, canGoRight = true, canGoLeft = true;


            
            //if (player.transform.position.x > rb.position.x || blockagesRight > 0)
            //    canGoRight = false;

            //if (player.transform.position.x < rb.position.x || blockagesLeft > 0)
            //    canGoLeft = false;

            //if (player.transform.position.y > rb.position.y || blockagesUp > 0)
            //    canGoUp = false;

            //if (player.transform.position.y < rb.position.y || blockagesDown > 0)
            //    canGoDown = false;

  
            


            if ( blockagesRight > 0)
                canGoRight = false;

            if ( blockagesLeft > 0)
                canGoLeft = false;

            if ( blockagesUp > 0)
                canGoUp = false;

            if ( blockagesDown > 0)
                canGoDown = false;


            bool[] directions = new bool[8]; //starting North and going clockwise

            for (int i = 0; i < 8; i++)
                directions[i] = false;

            if (canGoUp == true)
                directions[0] = true;
            if (canGoUp == true && canGoRight == true)
                directions[1] = true;

            if (canGoRight == true)
                directions[2] = true;
            if (canGoRight == true && canGoDown == true)
                directions[3] = true;

            if (canGoDown == true)
                directions[4] = true;
            if (canGoDown == true && canGoLeft == true)
                directions[5] = true;

            if (canGoLeft == true)
                directions[6] = true;
            if (canGoLeft == true && canGoUp == true)
                directions[7] = true;

            int dir = Random.Range(0, 8);

            while(directions[dir] == false)
                dir = Random.Range(0, 8);


            if (dir == 0 || dir == 1 || dir == 7)
                movement.y = 1;

            if (dir == 3 || dir == 4 || dir == 5)
                movement.y = -1;

            if (dir == 1 || dir == 2 || dir == 3)
                movement.x = 1;

            if (dir == 5 || dir == 6 || dir == 7)
                movement.x = -1;

        }

        */

        if (  ///player too far away
             Mathf.Sqrt(
           ((rb.position.x - player.position.x) * (rb.position.x - player.position.x)) +
           ((rb.position.y - player.position.y) * (rb.position.y - player.position.y))
           )
           > followDistance
            )
        {
            if (player.transform.position.x > rb.position.x + 0.1f)
                movement.x = 1f;
            if (player.transform.position.x < rb.position.x - 0.1f)
                movement.x = -1f;
            if (player.transform.position.y > rb.position.y + 0.1f)
                movement.y = 1f;
            if (player.transform.position.y < rb.position.y - 0.1f)
                movement.y = -1f;

            chooseTimer = .4f;
        }


        if (  ///player too close
             Mathf.Sqrt(
           ((rb.position.x - player.position.x) * (rb.position.x - player.position.x)) +
           ((rb.position.y - player.position.y) * (rb.position.y - player.position.y))
           )
           < keepDistance
            )
        {
            if (player.transform.position.x > rb.position.x + 0.1f)
                movement.x = -1f;
            if (player.transform.position.x < rb.position.x - 0.1f)
                movement.x = 1f;
            if (player.transform.position.y > rb.position.y + 0.1f)
                movement.y = -1f;
            if (player.transform.position.y < rb.position.y - 0.1f)
                movement.y = 1f;

            chooseTimer = .4f;
        }

        if( ///good, just small movement cause fun
                Mathf.Sqrt(
           ((rb.position.x - player.position.x) * (rb.position.x - player.position.x)) +
           ((rb.position.y - player.position.y) * (rb.position.y - player.position.y))
           ) >= keepDistance - 0.2f
           
           &&

           Mathf.Sqrt(
           ((rb.position.x - player.position.x) * (rb.position.x - player.position.x)) +
           ((rb.position.y - player.position.y) * (rb.position.y - player.position.y))
           ) <=followDistance + 0.2f
                )
        {
                chooseTimer = Random.Range(8, 12) / 10;
                movement.x = (float)(Random.Range(0, 3) - 1) * Random.Range(75,91) / 100;
                movement.y = (float)(Random.Range(0, 3) - 1) * Random.Range(75,91) / 100;
        }

        if (blockagesUp != 0 && movement.y == 1)
            movement.y = 0;
        if (blockagesDown != 0 && movement.y == -1)
            movement.y = 0;
        if (blockagesRight != 0 && movement.x == 1)
            movement.x = 0;
        if (blockagesLeft != 0 && movement.x == -1)
            movement.x = 0;

        if (movement.x == 0 && movement.y == 0)
        {
            if (blockagesUp != 0 && blockagesRight != 0)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                    movement.x = -1;
                if (rnd == 1)
                    movement.y = -1;

                chooseTimer = Random.Range(7, 13) / 10;


            }
            if (blockagesUp != 0 && blockagesLeft != 0)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                    movement.x = 1;
                if (rnd == 1)
                    movement.y = -1;

                    chooseTimer = Random.Range(7, 13) / 10;

            }
            if (blockagesDown != 0 && blockagesRight != 0)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                    movement.x = -1;
                if (rnd == 1)
                    movement.y = 1;
                chooseTimer = Random.Range(7, 13) / 10;

            }
            if (blockagesDown != 0 && blockagesLeft != 0)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                    movement.x = 1;
                if (rnd == 1)
                    movement.y = 1;
                chooseTimer = Random.Range(7, 13) / 10;
            }

           

        }

        if (movement.x != 0 && movement.y != 0)
        {
            int rnd = Random.Range(0, 4);

            if (rnd == 0)
                movement.x = 0;
            else
            {
                if (rnd == 1)
                    movement.y = 0;
                else
                {
                        rnd = Random.Range(0, 6);
                        if (rnd == 0)
                            movement.x /= 5;
                        if (rnd == 1)
                            movement.y /= 5;


                    }
            }
            

        }




            StartCoroutine(KeepMoving());

        }

        if(canFire == true)
        {
            float pX=0, pY=0;

            if (player.position.x > rb.position.x + 0.1f)
                pX = 1;
            if (player.position.x < rb.position.x - 0.1f)
                pX = -1;
            if (player.position.y > rb.position.y + 0.1f)
                pY = 1;
            if (player.position.y < rb.position.y - 0.1f)
                pY = -1;

            Projectile projectile =
            Instantiate(bullet , transform.position + 
                new Vector3(pX * ((rb.transform.localScale.x + bullet.transform.localScale.x) / 2 + .0f),
                pY * ((rb.transform.localScale.y + bullet.transform.localScale.y) / 2 + .0f), 0),
                Quaternion.identity);
            
            projectile.targetX = player.position.x;
            projectile.targetY = player.position.y;
            projectile.speed = bulletSpeed;
            projectile.lifeTime = bulletDecay;

            StartCoroutine(Shooting());
        }

    }


    IEnumerator KeepMoving()
    {
        reChoose = false;
        chooseTimer = 0.7f;
        yield return new WaitForSeconds(chooseTimer);
        reChoose = true;
    }

    IEnumerator Shooting()
    {
        canFire = false;
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }


    void FixedUpdate()
    {

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y == Camera.main.transform.position.y + 4.5f)
            blockagesUp++;

        if (collision.transform.position.y == Camera.main.transform.position.y - 4.5f)
            blockagesDown++;

        if (collision.transform.position.x == Camera.main.transform.position.x + 8.5f)
            blockagesRight++;

        if (collision.transform.position.x == Camera.main.transform.position.x - 8.5f)
            blockagesLeft++;



       


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.position.y == Camera.main.transform.position.y + 4.5f)
            blockagesUp--;

        if (collision.transform.position.y == Camera.main.transform.position.y - 4.5f)
            blockagesDown--;

        if (collision.transform.position.x == Camera.main.transform.position.x + 8.5f)
            blockagesRight--;

        if (collision.transform.position.x == Camera.main.transform.position.x - 8.5f)
            blockagesLeft--;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player Projectile"))
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();

            //Debug.Log("MONSTER HIT AND LOST " + proj.dmg + " HP ");
            HP -= proj.dmg;

            if (HP <= 0)
                HP = 0;

            if (HP == 0)
            {
                Destroy(this.gameObject);

            }
        }


    }

}
