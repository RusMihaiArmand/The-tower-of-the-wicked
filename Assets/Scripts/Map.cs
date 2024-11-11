using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] roomsOnMap;
    public GameObject[] mapParts;
    public GameObject playerIcon;

    void Start()
    {
        UpdateMap();
    }

    public void UpdateMap()
    {
        float x = Camera.main.transform.position.x;
        float y = Camera.main.transform.position.y;

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");

        string roomName = "map room ";

        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].transform.position.x == x && rooms[i].transform.position.y == y)
            {
                if (rooms[i].name.Contains("L"))
                    roomName += "L";
                if (rooms[i].name.Contains("U"))
                    roomName += "U";
                if (rooms[i].name.Contains("R"))
                    roomName += "R";
                if (rooms[i].name.Contains("D"))
                    roomName += "D";

            }
        }

        int visited = 0;

        visited = (int)((x / 18 + 3) + (-y / 10 + 3) * 7);

        roomsOnMap[visited].SetActive(true);
        playerIcon.transform.position = roomsOnMap[visited].transform.position;
        playerIcon.transform.position = new Vector3(playerIcon.transform.position.x, playerIcon.transform.position.y, -5);

        roomsOnMap[visited].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("map/" + roomName);

    }

    void Update()
    {
        


    }
}
