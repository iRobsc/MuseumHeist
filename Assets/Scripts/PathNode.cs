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
            "Costs: [G(" + g_cost + "), H(" + h_cost + "), F(" + GetFCost() + ")]\n\t" +
            "Parent: [" + parent.x + ", " + parent.y + "]";
    }

    public bool Equals(PathNode node) {
        return (this.x == node.x) && (this.y == node.y);
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

    public bool IsInList(List<PathNode> list) {

        for (int i = 0; i < list.Count; i++)
        {
            PathNode current = list[i];
            if (current.x == this.x && current.y == this.y) {
                return true;
            }
        }
        return false;

    }

}