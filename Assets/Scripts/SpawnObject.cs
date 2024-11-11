using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject[] objects;
    public bool visited = false;

    private void Start()
    {
        if(!this.gameObject.name.Contains("Spawner"))
        {
            int rand = Random.Range(0, objects.Length);
            float bossPosX, bossPosY;

            GameObject gms = GameObject.FindGameObjectWithTag("GameController");
            bossPosX = gms.GetComponent<GameStateManager>().GetBossPosX();
            bossPosY = gms.GetComponent<GameStateManager>().GetBossPosY();

            if (objects[rand].tag.Contains("Door") &&
                (
                    (((transform.position.x == bossPosX + 9.5f) ||
                    (transform.position.x == bossPosX - 9.5f)) &&
                    ((transform.position.y == bossPosY + 0.5f) ||
                    (transform.position.y == bossPosY - 0.5f)))
                    ||
                    (((transform.position.x == bossPosX + 0.5f) ||
                    (transform.position.x == bossPosX - 0.5f)) &&
                    ((transform.position.y == bossPosY + 5.5f) ||
                    (transform.position.y == bossPosY - 5.5f)))
                    )) 
            {
                GameObject openDoor = Resources.Load<GameObject>("Prefabs/tiles/Open Doors/" +
                    objects[rand].GetComponent<SpriteRenderer>().sprite.name + " BOSS");

                Instantiate(openDoor,
                    new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);
            }
            else
                Instantiate(objects[rand], new Vector3(transform.position.x, transform.position.y, 1), Quaternion.identity);

            Destroy(this.gameObject);
        }
        
    }

    private void Update()
    {
        if (visited == false &&
            Camera.main.transform.position.x - 8.5f <= transform.position.x &&
            transform.position.x <= Camera.main.transform.position.x + 8.5f &&
            Camera.main.transform.position.y - 4.5f <= transform.position.y &&
            transform.position.y <= Camera.main.transform.position.y + 4.5f &&
            Camera.main.transform.position.x % 18 == 0 &&
            Camera.main.transform.position.y % 10 == 0
            )
        {
            visited = true;
            int rand = Random.Range(0, objects.Length);
            Instantiate(objects[rand], transform.position, Quaternion.identity);
            Destroy(this.gameObject);

        }
    }

}
