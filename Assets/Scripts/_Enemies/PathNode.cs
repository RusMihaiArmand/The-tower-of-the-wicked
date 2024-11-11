using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathNode{

    private Grid grid;
    private int x;
    private int y;

    public int gCost;
    public int hCost;
    public int fCost;
    public bool blocked = false;

    public PathNode cameFromNode;

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public PathNode(Grid g, int X, int Y)
    {
        this.grid = g;
        this.x = X;
        this.y = Y;
        this.blocked = false;
    }

    public override string ToString()
    {
        return x + " , " + y;
    }

    public void CalculateFCost()
    {
        fCost = hCost + gCost;
    }

}
