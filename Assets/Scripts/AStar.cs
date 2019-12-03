using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{

    //public PathNode startingNode;
    //public PathNode targetNode;

    public Tilemap tilemap;

    private Vector3Int size;

    private void Awake()
    {
        size = tilemap.size;
    }

    private void Update()
    {
        FindPath(new PathNode(0, 0), new PathNode(5, 5));
    }

    public List<PathNode> FindPath(PathNode startingNode, PathNode targetNode)
    {

        List<PathNode> openNodes = new List<PathNode>();
        List<PathNode> closedNodes = new List<PathNode>();

        openNodes.Add(startingNode);
        PathNode currentNode = openNodes[0];
        Debug.Log("DING;DING;DING");
        while (!targetNode.Equals(currentNode)) {
            
            currentNode = openNodes[0];

            //checking openNodes and finding next current node
            for (int i = 1; i < openNodes.Count; i++)
            {
                PathNode openNode = openNodes[i];
                if ((openNode.GetFCost() < currentNode.GetFCost()) || (openNode.GetFCost() == currentNode.GetFCost() && openNode.h_cost < currentNode.h_cost))
                {
                    currentNode = openNode;
                }
            }

            //adding and removing
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            //creating neighbours
            List<PathNode> neighbours = GetNeighbours(currentNode);

            for (int i = 0; i < neighbours.Count; i++)
            {
                PathNode neighbour = neighbours[i];
                if (!neighbour.IsInList(closedNodes))
                { //&& Tile is walkable

                    int newCost = currentNode.g_cost + currentNode.GetDistanceTo(neighbour);
                    if (newCost < neighbour.g_cost || !neighbour.IsInList(openNodes))
                    {

                        neighbour.g_cost = newCost;
                        neighbour.h_cost = neighbour.GetDistanceTo(targetNode);
                        neighbour.parent = currentNode;

                        if (!neighbour.IsInList(openNodes))
                        {
                            openNodes.Add(neighbour);
                            Debug.Log("NEIGH" + neighbour.ToString());
                        }
                    }

                }
            }

        }

        Debug.Log("DONE;DONE;DONE");

        RetracePath(targetNode, startingNode);

        return null;
    }

    List<PathNode> GetNeighbours(PathNode node) {

        List<PathNode> neighbours = new List<PathNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighbourX = node.x + i;
                int neighbourY = node.y + j;

                if (neighbourX >= 0 && neighbourX < size.x && neighbourY >= 0 && neighbourY < size.y) {
                    PathNode neighbour = new PathNode(neighbourX, neighbourY);
                    neighbour.g_cost = 9999999;
                    neighbours.Add(neighbour);
                }

            }
        }

        return neighbours;
    
    }

    void RetracePath(PathNode from, PathNode to)
    {

        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = to;

        Debug.Log(to.parent);
        Debug.Log(to.x);
        Debug.Log(to.y);
        Debug.Log(from.parent);
        Debug.Log(from.x);
        Debug.Log(from.y);

        while (currentNode.x != from.x && currentNode.y != from.y)
        {
            Debug.Log("Hello????");
            currentNode = currentNode.parent;
            currentNode = from;
        }

        Debug.Log("Hello?");

        path.Reverse();

        foreach (PathNode waypoint in path)
        {
            Debug.Log("WAYPOINT " + waypoint.ToString());
        }

    }

}
