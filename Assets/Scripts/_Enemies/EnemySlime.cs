using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Transform player;
    public int HP = 8, maxHP = 8;
    public int damage = 2;
    private bool spawnedSmol = false;
    private float invincibleTime = 0f;
    private bool invincible = false;
    private bool dying = false;

    public Animator animator;
    public bool found;
    Vector2 movement;
    private List<PathNode> correctPath;

    private Grid grid;


    

    // Update is called once per frame
    private void Start()
    {
        animator.Play("Walk");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        grid = new Grid(18*2, 10*2, 0.5f, Camera.main.transform.position - new Vector3(9,5,0));
        found = false;
        StartCoroutine(InvincibilityCooldown());
    }

    IEnumerator InvincibilityCooldown()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;
    }


    void Update()
    {
        if (dying)
        {
            animator.Play("Dead");
            return;
        }

        if (Mathf.Abs(this.transform.position.x - player.position.x) < 1
            && Mathf.Abs(this.transform.position.y - player.position.y) < 1)
        {

            if (this.transform.position.x < player.position.x)
                movement.x = 1;
            if (this.transform.position.x > player.position.x)
                movement.x = -1;

            if (this.transform.position.y < player.position.y)
                movement.y = 1;
            if (this.transform.position.y > player.position.y)
                movement.y = -1;

        }
        else
        {
            if(found == false)
            {
                grid.GetXY(this.transform.position, out int startX, out int startY);
                grid.GetXY(player.transform.position, out int endX, out int endY);

                grid.SetNotBlocked();
                GameObject[] obstacles = GameObject.FindGameObjectsWithTag("PathBlocker");

                foreach(GameObject obstacle in obstacles)
                {
                    grid.GetXY(obstacle.transform.position, out int ox, out int oy);
                    PathNode pn = grid.GetGridObject(ox, oy);

                    if(pn != null)
                    {
                        pn.blocked = true;
                    }

                }

                correctPath = GameStateManager.Instance.FindPath(grid, startX, startY, endX, endY);
                
                found = true;
                StartCoroutine(GetNewPath());
            }

        }

 
    }

    IEnumerator KeepMoving()
    {
        if(!dying)
        {
            animator.Play("Hurt");
            yield return new WaitForSeconds(0.1f);
            animator.Play("Walk");
            found = false;


        }
        
    }

    IEnumerator GetNewPath()
    {
        //float timer = Random.Range(5, 15)/10 ;
        float timer = 0.4f;
        yield return new WaitForSeconds(timer);
        found = false;
    }

    IEnumerator Die()
    {
        dying = true;
        movement.x = 0;
        movement.y = 0;
        Destroy(this.GetComponent<BoxCollider2D>());
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        animator.Play("Dead");
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }



    void FixedUpdate()
    {
        if(dying)
            return;

        int targetX, targetY;
        grid.GetXY(this.transform.position, out int currentX, out int currentY);
        if(!(correctPath is null) && correctPath.Count > 0)
        {
            PathNode pn = correctPath[0];

            targetX = pn.GetX();
            targetY = pn.GetY();

            if (currentX != targetX || currentY != targetY)
            {
                if (currentX < targetX)
                    movement.x = 1;
                if (currentX > targetX)
                    movement.x = -1;
                if (currentX == targetX)
                    movement.x = 0;

                if (currentY < targetY)
                    movement.y = 1;
                if (currentY > targetY)
                    movement.y = -1;
                if (currentY == targetY)
                    movement.y = 0;

                grid.GetXY(this.transform.position, out currentX, out currentY);
            }
            else
            {
                correctPath.Remove(pn);
            }

        }


        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dying)
            return;
        if (collision.tag.Equals("Player Projectile"))
        {
            Projectile proj = collision.gameObject.GetComponent<Projectile>();

            if(!invincible)
                HP -= proj.dmg;
            found = true;
            //correctPath = null;

            if (proj.transform.position.x > this.transform.position.x)
                movement.x = -1;
            if (proj.transform.position.x < this.transform.position.x)
                movement.x = 1;
            if (proj.transform.position.x == this.transform.position.x)
                movement.x = 0;

            if (proj.transform.position.y > this.transform.position.y)
                movement.y = -1;
            if (proj.transform.position.y < this.transform.position.y)
                movement.y = 1;
            if (proj.transform.position.y == this.transform.position.y)
                movement.y = 0;

            StartCoroutine(KeepMoving());

            if (HP <= 0)
                HP = 0;

            if (HP == 0)
            {
                
                if (spawnedSmol == false)
                {
                    spawnedSmol = true;
                    if (this.maxHP > 2)
                        for (int iS = 1; iS <= 2; iS++)
                        {
                            GameObject s1 = Instantiate(Resources.Load<GameObject>("Prefabs/enemies/Slime"),
                                                    this.transform.position,
                                                    Quaternion.identity);
                            s1.GetComponent<EnemySlime>().movement = this.movement;

                            if (this.maxHP == 8)
                            {
                                s1.GetComponent<EnemySlime>().maxHP = 4;
                                s1.GetComponent<EnemySlime>().HP = 4;
                                s1.GetComponent<EnemySlime>().damage = 2;
                                s1.GetComponent<EnemySlime>().moveSpeed = 2.8f;
                                s1.transform.localScale = new Vector3(0.3f, 0.3f);
                            }

                            if (this.maxHP == 4)
                            {
                                s1.GetComponent<EnemySlime>().maxHP = 2;
                                s1.GetComponent<EnemySlime>().HP = 2;
                                s1.GetComponent<EnemySlime>().damage = 1;
                                s1.GetComponent<EnemySlime>().moveSpeed = 3f;
                                s1.transform.localScale = new Vector3(0.2f, 0.2f);
                            }
                            s1.GetComponent<EnemySlime>().invincible = true;
                            s1.GetComponent<EnemySlime>().invincibleTime = 0.5f;
                        }
                    if (maxHP == 2)
                        StartCoroutine(Die());
                    else
                        Destroy(this.gameObject);
                }
                //if(maxHP != 2)
                //    StartCoroutine(Die());
                //else
                //    Destroy(this.gameObject);
            }
        }


        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (dying)
            return;
        if (collision.collider.tag.Equals("Player"))
        {
            movement.x = 0;
            movement.y = 0;
            player.GetComponent<PlayerMovement>().TakenDamage(this.damage);
        }
    }

}
