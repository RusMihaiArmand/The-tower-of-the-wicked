using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    public GameState gamestate = GameState.Gameplay;
    public GameObject pauseMenu;
    public GameObject statsMenu;
    public GameObject comboMenu;
    private bool paused = false;
    private bool showStats = false;
    private bool showCombos = false;
    public Text[] stats = new Text[8];
    public GameObject quoteCanvasEndQuote;

    private float bossPosX,bossPosY;

    public void Start()
    {
        _instance = this;
    }


    private List<PathNode> openList;
    private List<PathNode> closedList;

    //!
    public List<PathNode> FindPath(Grid grid, int startX, int startY, int endX, int endY)
    {
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathN = grid.GetGridObject(x, y);
                pathN.gCost = 999;
                pathN.CalculateFCost();
                pathN.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();


        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbour in GetNeighbours(grid, currentNode))
            {
                if (closedList.Contains(neighbour))
                    continue;

                if (neighbour.blocked == true &&   (neighbour.GetX() != endX || neighbour.GetY() != endY)  )
                {
                    closedList.Add(neighbour);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbour);
                if (tentativeGCost < neighbour.gCost)
                {
                    neighbour.cameFromNode = currentNode;
                    neighbour.gCost = tentativeGCost;
                    neighbour.hCost = CalculateDistanceCost(neighbour, endNode);
                    neighbour.CalculateFCost();

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }


        return null;

    }

    private List<PathNode> CalculatePath(PathNode endN)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endN);
        PathNode currentNode = endN;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private List<PathNode> GetNeighbours(Grid grid, PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();

        if (currentNode.GetX() - 1 >= 0)
        {
            //L
            neighbours.Add(GetNode(grid, currentNode.GetX() - 1, currentNode.GetY()));
            //L D
            if (currentNode.GetY() - 1 >= 0)
                neighbours.Add(GetNode(grid, currentNode.GetX() - 1, currentNode.GetY() - 1));
            //L U
            if (currentNode.GetY() + 1 < grid.GetHeight())
                neighbours.Add(GetNode(grid, currentNode.GetX() - 1, currentNode.GetY() + 1));
        }

        if (currentNode.GetX() + 1 < grid.GetWidth())
        {
            //R
            neighbours.Add(GetNode(grid, currentNode.GetX() + 1, currentNode.GetY()));
            //R D
            if (currentNode.GetY() - 1 >= 0)
                neighbours.Add(GetNode(grid, currentNode.GetX() + 1, currentNode.GetY() - 1));
            //R U
            if (currentNode.GetY() + 1 < grid.GetHeight())
                neighbours.Add(GetNode(grid, currentNode.GetX() + 1, currentNode.GetY() + 1));
        }

        //D
        if (currentNode.GetY() - 1 >= 0)
            neighbours.Add(GetNode(grid, currentNode.GetX(), currentNode.GetY() - 1));
        //U
        if (currentNode.GetY() + 1 < grid.GetHeight())
            neighbours.Add(GetNode(grid, currentNode.GetX(), currentNode.GetY() + 1));

        return neighbours;
    }

    private PathNode GetNode(Grid grid, int x, int y)
    {
        return grid.GetGridObject(x, y);

    }

    private int CalculateDistanceCost(PathNode A, PathNode B)
    {
        int xDist = Mathf.Abs(A.GetX() - B.GetX());
        int yDist = Mathf.Abs(A.GetY() - B.GetY());
        int remaining = Mathf.Abs(xDist - yDist);

        return 14 * Mathf.Min(xDist, yDist) + 10 * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pnList)
    {
        PathNode lowestFCostN = pnList[0];
        for (int i = 1; i < pnList.Count; i++)
        {
            if (pnList[i].fCost < lowestFCostN.fCost)
            {
                lowestFCostN = pnList[i];
            }
        }
        return lowestFCostN;
    }



    public static GameStateManager Instance
    {

        get
        {
            if (_instance == null)
                _instance = new GameStateManager();

            return _instance;

        }

    }

    public void PauseGame()
    {
        paused = !paused;

        if (paused)
        {
            showStats = false;
            statsMenu.SetActive(false);
            showCombos = false;
            comboMenu.SetActive(false);

            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            gamestate = GameState.Paused;
        }
        else
        {
            statsMenu.SetActive(false);
            comboMenu.SetActive(false);

            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            gamestate = GameState.Gameplay;
        }
    }

    public void StatsWindow()
    {
        showStats = !showStats;

        if(showStats)
        {
            pauseMenu.SetActive(false);
            statsMenu.SetActive(true);

            PlayerMovement player = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<PlayerMovement>();

            stats[0].text = "" + player.GetStats("MaxHP");
            stats[1].text = "" + player.GetStats("Speed");
            stats[2].text = "" + player.GetStats("BulletNumber");
            stats[3].text = "" + player.GetStats("BulletSpeed");
            stats[4].text = "" + player.GetStats("BulletSeparation");
            stats[5].text = "" + player.GetStats("BulletTime");
            stats[6].text = "" + player.GetStats("BulletDmg");
            stats[7].text = "" + player.GetStats("MeleeDmg");

        }
        else
        {
            pauseMenu.SetActive(true);
            statsMenu.SetActive(false);
        }
    }

    public void ComboWindow()
    {
        showCombos = !showCombos;

        if (showCombos)
        {
            pauseMenu.SetActive(false);
            comboMenu.SetActive(true);

            

        }
        else
        {
            pauseMenu.SetActive(true);
            comboMenu.SetActive(false);
        }
    }


    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            PauseGame();
        }

    }

    public void SetBossPos(float X, float Y)
    {
        bossPosX = X;
        bossPosY = Y;
    }

    public float GetBossPosX()
    {
        return bossPosX;
    }

    public float GetBossPosY()
    {
        return bossPosY;
    }

    private GameStateManager()
    {

    }

}
