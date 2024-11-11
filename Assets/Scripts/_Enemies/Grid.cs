using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private PathNode[,] gridArray;
    private float cellSize;

    private Vector3 originPosition;

    public Grid(int w, int h, float cs, Vector3 op)
    {
        this.width = w;
        this.height = h;
        this.cellSize = cs;
        this.originPosition = op;

        this.gridArray = new PathNode[w, h];

        for (int i = 0; i < w; i++)
            for (int j = 0; j < h; j++)
            {
                gridArray[i, j] = new PathNode(this, i, j);
            }


        //for (int i=0; i<w;i++)
        //    for(int j=0; j<h; j++)
        //    {
        //        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
        //        Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i+1 , j ), Color.white, 100f);
        //    }



        //Debug.DrawLine(GetWorldPosition(w, 0), GetWorldPosition(w, h), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(0, h), GetWorldPosition(w, h), Color.white, 100f);
        

    }
  
    public int GetWidth()
    {
        return this.width;
    }

    public int GetHeight()
    {
        return this.height;
    }

    public void SetNotBlocked()
    {
        for (int x = 0; x < this.width; x++)
            for (int y = 0; y < this.height; y++)
                gridArray[x, y].blocked = false;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + this.originPosition;
    }

    public void SetGridObject(int x, int y, PathNode val)
    {
        if (x >= 0 && x < this.width && y >= 0 && y < this.height)
        {
            gridArray[x, y] = val;
        }
    }

    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - this.originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPos - this.originPosition).y / cellSize);

    }

    public PathNode GetGridObject(int x, int y)
    {
        if (x >= 0 && x < this.width && y >= 0 && y < this.height)
        {
            return gridArray[x, y];
        }
        else
        {
            return null;
        }
            
    }

    public PathNode GetGridObject(Vector3 worldPos)
    {
        GetXY(worldPos, out int x, out int y);

        if (x >= 0 && x < this.width && y >= 0 && y < this.height)
        {
            return gridArray[x, y];
        }
        else
            return null;
    }

    public void SetGridObject(Vector3 worldPos, PathNode val)
    {
        GetXY(worldPos, out int x, out int y);
        SetGridObject(x, y, val);
    }

}
