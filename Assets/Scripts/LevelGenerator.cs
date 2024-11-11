using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] positions;
    public RoomOption[] rooms;
    public Transform player;
    public int[] roomType = new int[49];
    public int[] roomCluster = new int[49];


    [System.Serializable]
    public class RoomOption
    {
        public GameObject room;
        public int roomWeight;
        
        public RoomOption(GameObject r, int rw)
        {
            this.room = r;
            this.roomWeight = rw;
        }
    }

    private void Check(int roomNr)
    {
        if (roomType[roomNr] != -1)
        {
            int l = (int)Mathf.Sqrt(positions.Length);



            int[] options = { -1, -1, -1, -1 };
            int nrOpt = 0;

            if (roomNr >= l)
                if ((roomCluster[roomNr - l] != -1)  && (Random.Range(0,10)==4 || roomCluster[roomNr - l] != roomCluster[roomNr])   )
                {
                    if(roomCluster[roomNr - l] == roomCluster[roomNr])
                    {
                        if ( (((roomType[roomNr] / 2) / 2) % 2)==0 )
                        {
                            options[nrOpt] = roomNr - l;
                            nrOpt++;
                        }

                    }
                    else
                    {
                        options[nrOpt] = roomNr - l;
                        nrOpt++;
                    }
                    
                }

            if (roomNr + l < positions.Length)
                if ((roomCluster[roomNr + l] != -1) && (Random.Range(0, 10) == 4 || roomCluster[roomNr + l] != roomCluster[roomNr]))
                {
                    if (roomCluster[roomNr + l] == roomCluster[roomNr])
                    {
                        if ((roomType[roomNr] % 2) == 0)
                        {
                            options[nrOpt] = roomNr + l;
                            nrOpt++;
                        }

                    }
                    else
                    {
                        options[nrOpt] = roomNr + l;
                        nrOpt++;
                    }
                }

            if (roomNr % l != 0)
                if ((roomCluster[roomNr - 1] != -1) && (Random.Range(0, 10) == 4 || roomCluster[roomNr - 1] != roomCluster[roomNr]))
                {
                    if (roomCluster[roomNr - 1] == roomCluster[roomNr])
                    {
                        if (((((roomType[roomNr] / 2) / 2) / 2) % 2) == 0)
                        {
                            options[nrOpt] = roomNr - 1;
                            nrOpt++;
                        }

                    }
                    else
                    {
                        options[nrOpt] = roomNr - 1;
                        nrOpt++;
                    }
                }

            if ((roomNr + 1) % l != 0)
                if ((roomCluster[roomNr + 1] != -1) && (Random.Range(0, 10) == 4 || roomCluster[roomNr + 1] != roomCluster[roomNr]))
                {
                    if (roomCluster[roomNr + 1] == roomCluster[roomNr])
                    {
                        if (((roomType[roomNr] / 2) % 2) == 0)
                        {
                            options[nrOpt] = roomNr + 1;
                            nrOpt++;
                        }

                    }
                    else
                    {
                        options[nrOpt] = roomNr + 1;
                        nrOpt++;
                    }
                }

            if (nrOpt != 0)
            {
                int selected = options [Random.Range(0, nrOpt) ];

                /// +8 -> door to left
                /// +4 -> door to up
                /// +2 -> door to right
                /// +1 -> door to down

                ///generate door to Left
                if (roomNr - selected == 1)
                {
                    roomType[roomNr] += 8;
                    roomType[selected] += 2;
                }

                ///generate door to up
                if (roomNr - selected == l)
                {
                    roomType[roomNr] += 4;
                    roomType[selected] += 1;
                }

                ///generate door to right
                if (selected - roomNr == 1)
                {
                    roomType[roomNr] += 2;
                    roomType[selected] += 8;
                }

                ///generate door to up
                if (selected - roomNr == l)
                {
                    roomType[roomNr] += 1;
                    roomType[selected] += 4;
                }

                Changing(roomCluster[roomNr], roomCluster[selected]);
              
            }




        }

    }

    private void Changing(int c1, int c2)
    {
        int changer, changed;
        if (c1 < c2)
        {
            changer = c1;
            changed = c2;
        }
        else
        {
            changed = c1;
            changer = c2;
        }

        if (c1 != c2)
            for (int i = 0; i < 49; i++)
                if (roomCluster[i] == changed)
                    roomCluster[i] = changer;

    }

    private void Start()
    {
        for (int i = 0; i < 49; i++)
        {
            roomType[i] = -1;
            roomCluster[i] = -1;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;


        int startPos = Random.Range(0, positions.Length);
        int bossPos = Random.Range(0, positions.Length);

        while(bossPos == startPos)
            bossPos = Random.Range(0, positions.Length);

        roomType[startPos] = 0;
        roomCluster[startPos] = 1;


        player.position = new Vector3(positions[startPos].position.x, positions[startPos].position.y, 0);
        Camera.main.transform.position = new Vector3(positions[startPos].position.x, positions[startPos].position.y, -10);
        // player.cam

        int nrRooms = Random.Range(16, 22);
        int roomsCreated = 0;

        /// throw rooms in there
        while(roomsCreated < nrRooms)
        {
            int i = Random.Range(0, positions.Length);
            while (roomType[i] == -1)
                i = Random.Range(0, positions.Length);

            int j;

            j = i;
            while (j < positions.Length && roomsCreated < nrRooms)
            {
                if (roomType[j] != -1)
                {

                    int[] options = { -1, -1, -1, -1 };
                    int nrOpt = 0;
                    int l = (int)Mathf.Sqrt(positions.Length);


                    if (j >= l)
                        if (roomType[j - l] == -1)
                        {
                           options[nrOpt] = j - l;
                           nrOpt++;
                        }

                    if (j + l < positions.Length)
                        if (roomType[j + l] == -1)
                         {
                           options[nrOpt] = j + l;
                          nrOpt++;
                         }

                     if (j % l != 0)
                         if (roomType[j - 1] == -1)
                         {
                          options[nrOpt] = j - 1;
                          nrOpt++;
                          }

                    if ((j + 1) % l != 0)
                        if (roomType[j + 1] == -1)
                        {
                            options[nrOpt] = j + 1;
                            nrOpt++;
                        }

                    if (nrOpt != 0)
                    {
                        roomsCreated++;
                        int selected = Random.Range(0, nrOpt);
                        roomType[options[ selected  ]] = 0;
                        roomCluster[options[selected]] = roomsCreated + 1;
                    }


                }
                j++;
            }

            j = 0;
            while (j < i && roomsCreated < nrRooms)
            {
                if (roomType[j] != -1)
                {

                    int[] options = { -1, -1, -1, -1 };
                    int nrOpt = 0;
                    int l = (int)Mathf.Sqrt(positions.Length);

                    if (j >= l)
                        if (roomType[j - l] == -1)
                        {
                            options[nrOpt] = j - l;
                            nrOpt++;
                        }

                    if (j + l < positions.Length)
                        if (roomType[j + l] == -1)
                        {
                            options[nrOpt] = j + l;
                            nrOpt++;
                        }

                    if (j % l != 0)
                        if (roomType[j - 1] == -1)
                        {
                            options[nrOpt] = j - 1;
                            nrOpt++;
                        }

                    if ((j + 1) % l != 0)
                        if (roomType[j + 1] == -1)
                        {
                            options[nrOpt] = j + 1;
                            nrOpt++;
                        }

                    if (nrOpt != 0)
                    {
                        roomsCreated++;
                        int selected = Random.Range(0, nrOpt);
                        roomType[options[selected]] = 0;
                        roomCluster[options[selected]] = roomsCreated + 1;
                    }
   
                }
                j++;
            }




        }




        /// actual doors between rooms
         
        bool good = false;
        while (good == false)
        {
            int location1;

            location1 = Random.Range(0, positions.Length);
            while (roomCluster[location1] == -1)
                location1 = Random.Range(0, positions.Length);

            Check(location1);

            good = true;
            for (int i = 0; i < positions.Length; i++)
            {
                if (roomCluster[i] != 1 && roomCluster[i] != -1)
                    good = false;
            }

        }

        ///      REPEAT: 8-L   4-U   2-R   1-D   MEANING:
        ///    1-D       2-R       3-RD         4-U       5-UD
        ///    6-UR      7-URD     8-L          9-LD      10-LR
        ///    11-LRD    12-LU     13-LUD       14-LUR    15-LURD 


        /// new stuff for BOSS ROOM

        int ok = 0;

        while (ok==0)
        {
            int l = (int)Mathf.Sqrt(positions.Length);

            bossPos = Random.Range(0, positions.Length);
            while (roomType[bossPos] != -1 ||
                startPos == bossPos + 1   ||  startPos == bossPos - 1 ||
                startPos == bossPos + l   ||  startPos == bossPos - l)
                bossPos = Random.Range(0, positions.Length);



            int[] options = { -1, -1, -1, -1 };
            int nrOpt = 0;


            if (bossPos >= l)
                if (roomType[bossPos - l] != -1)
                {
                    options[nrOpt] = bossPos - l;
                    nrOpt++;
                }

            if (bossPos + l < positions.Length)
                if (roomType[bossPos + l] != -1)
                {
                    options[nrOpt] = bossPos + l;
                    nrOpt++;
                }

            if (bossPos % l != 0)
                if (roomType[bossPos - 1] != -1)
                {
                    options[nrOpt] = bossPos - 1;
                    nrOpt++;
                }

            if ((bossPos + 1) % l != 0)
                if (roomType[bossPos + 1] != -1)
                {
                    options[nrOpt] = bossPos + 1;
                    nrOpt++;
                }

            if (nrOpt != 0)
            {
                ok = 1;

                int selected = Random.Range(0, nrOpt);


                if(options[selected] == bossPos - 1)
                {
                    roomType[bossPos] = 8;
                    roomType[options[selected]] += 2;
                }

                if (options[selected] == bossPos + 1)
                {
                    roomType[bossPos] = 2;
                    roomType[options[selected]] += 8;
                }


                if (options[selected] == bossPos + l)
                {
                    roomType[bossPos] = 1;
                    roomType[options[selected]] += 4;
                }

                if (options[selected] == bossPos - l)
                {
                    roomType[bossPos] = 4;
                    roomType[options[selected]] += 1;
                }


                roomCluster[bossPos] = roomCluster[options[selected]];
            }


  


        }



        ///BOSS ROOM ends here


        for (int i = 0; i < positions.Length; i++)
            if (roomType[i] != -1)
            {
                int hasL, hasR, hasU, hasD;

                hasD = roomType[i] % 2;
                hasR = (roomType[i] / 2) % 2;
                hasU = ((roomType[i] / 2) /2) % 2;
                hasL = (((roomType[i] / 2) / 2) /2) % 2;

                int[] options = new int[50];
                int nrOpt = 0;

                for(int j=0; j< rooms.Length; j++)
                {
                    if(
                        ( (hasD==1 && rooms[j].room.name.Contains("D")) || (hasD == 0 && !rooms[j].room.name.Contains("D"))) &&
                        ((hasR == 1 && rooms[j].room.name.Contains("R")) || (hasR == 0 && !rooms[j].room.name.Contains("R"))) &&
                        ((hasU == 1 && rooms[j].room.name.Contains("U")) || (hasU == 0 && !rooms[j].room.name.Contains("U"))) &&
                        ((hasL == 1 && rooms[j].room.name.Contains("L")) || (hasL == 0 && !rooms[j].room.name.Contains("L"))) &&
                        (rooms[j].room.name.Contains("SPAWN") == (startPos == i)) &&
                        (rooms[j].room.name.Contains("BOSS") == (bossPos == i))
                        )
                    {

                        for(int roomopts = 1; roomopts <= rooms[j].roomWeight; roomopts++)
                        {
                            options[nrOpt] = j;
                            nrOpt++;

                        }
                        
                    }



                }
                Instantiate(rooms[ options[Random.Range(0, nrOpt)]       ].room, positions[i].position, Quaternion.identity);
            }

        GameObject gsm = GameObject.FindGameObjectWithTag("GameController");
        gsm.GetComponent<GameStateManager>().SetBossPos(positions[bossPos].transform.position.x, positions[bossPos].transform.position.y);

        //Vector3 gridP = Camera.main.transform.position - new Vector3(9, 5, 0);
        //Grid gr = new Grid(18, 10, 1f,  gridP);
        
    }

}
