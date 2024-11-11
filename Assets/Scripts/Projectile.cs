using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    public float targetX = 0, targetY = 0;
    public float speed = 0, lifeTime = 1;
    public int dmg = 1;

    void Start()
    {
        StartCoroutine(Decay());
        targetX = targetX + (targetX - transform.position.x) * 10;
        targetY = targetY + (targetY - transform.position.y) * 10;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            new Vector2(targetX , targetY ), speed * Time.deltaTime);
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag.Equals("Wall") || collision.collider.tag.Equals("Player") || collision.collider.tag.Equals("Enemy"))
        {
            Destroy(this.gameObject);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Wall") || (collision.tag.Equals("Player") && !transform.tag.Equals("Player Projectile")) || collision.tag.Equals("Enemy"))
        {
            Destroy(this.gameObject);

        }
    }

}
