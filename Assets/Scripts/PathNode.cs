using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : Object
{

    public int x; //x position
    public int y; //y position
    public int g_cost; //distance from start node
    public int h_cost; //distance from end node
    public int f_cost; //g + h cost

    public PathNode parent;

    public PathNode(int x, int y)
    {

        this.x = x;
        this.y = y;

    }

    override
    public System.String ToString()
    {
        return "Position: [" + this.x + ", " + this.y + "]\n\t" +
            "Costs: [G(" + g_cost + "), H(" + h_cost + "), F(" + GetFCost() + ")]";
    }

    public int GetFCost()
    {
        return this.g_cost + this.h_cost;
    }

    public int GetDistanceTo(PathNode to)
    {

        int distX = Mathf.Abs(this.x - to.x);
        int distY = Mathf.Abs(this.y - to.y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            return 14 * distX + 10 * (distY - distY);
        }
    }

    public bool isDiagonalTo(PathNode to)
    {

        bool xChanged = !(this.x == to.x);
        bool yChanged = !(this.y == to.y);

        return xChanged && yChanged;

    }

}