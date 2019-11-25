using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    void Start() {
        PathNode startingNode = new PathNode(0,0);
        PathNode targetNode = new PathNode(10,10);

        List<PathNode> neighbours = GetNeighbours(startingNode);

        while (startingNode.x != targetNode.x && startingNode.x != targetNode.x) {

            //Debug.Log("WHILE");

        }

    }

    List<PathNode> GetNeighbours(PathNode node)
    {
        List<PathNode> neighbours = new List<PathNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                //center node == current node
                if (i == 0 && j == 0) {
                    continue;
                }

                int checkX = node.x + i;
                int checkY = node.y + j;

                if (checkX > 0 && checkY > 0) {
                    PathNode neighbour = new PathNode(checkX, checkY);
                    Debug.Log(neighbour.ToString());
                    neighbours.Add(neighbour);
                }

            }
        }

        return neighbours;
    }

    int GetDistance(PathNode start, PathNode end) {
        return 0;
    }

}

public class PathNode {

    public int x;
    public int y;
    public int h_cost;
    public int g_cost;
    public int f_cost;

    public PathNode(int x, int y) {
        this.x = x;
        this.y = y;
    }

    override
    public string ToString() { 
        return "Position [" + x + ", " + y + "]";
    }

}