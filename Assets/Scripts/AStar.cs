using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{

    PathNode startingNode;
    PathNode targetNode;

    List<PathNode> openNodes;
    List<PathNode> closedNodes;

    public Tilemap tilemap;

    private Vector3Int size;

    private void Awake()
    {
        size = tilemap.size;
    }

    public void Update()
    {
        startingNode = new PathNode(0, 0);
        targetNode = new PathNode(3, 3);
        FindPath(startingNode, targetNode);
    }

    public List<PathNode> FindPath(PathNode startingNode, PathNode targetNode)
    {

        openNodes = new List<PathNode>();
        closedNodes = new List<PathNode>();

        openNodes.Add(startingNode);
        PathNode currentNode = openNodes[0];

        while (!targetNode.Equals(currentNode))
        {

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
                        neighbour.SetParent(currentNode);

                        if (!neighbour.IsInList(openNodes))
                        {
                            openNodes.Add(neighbour);
                        }
                    }

                }
            }

        }

        return RetracePath(targetNode, startingNode);
    }

    List<PathNode> GetNeighbours(PathNode node)
    {

        List<PathNode> neighbours = new List<PathNode>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighbourX = node.x + i;
                int neighbourY = node.y + j;

                if (neighbourX >= 0 && neighbourX < size.x && neighbourY >= 0 && neighbourY < size.y)
                {
                    PathNode neighbour = new PathNode(neighbourX, neighbourY);
                    neighbour.g_cost = neighbour.GetDistanceTo(startingNode);
                    neighbour.h_cost = neighbour.GetDistanceTo(targetNode);
                    neighbour.GetFCost();
                    neighbours.Add(neighbour);
                }

            }
        }

        return neighbours;

    }

    List<PathNode> RetracePath(PathNode from, PathNode to)
    {

        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = new PathNode(0, 0);

        for (int i = 0; i < closedNodes.Count; i++)
        {
            if (closedNodes[i].x == from.x && closedNodes[i].y == from.y)
            {
                currentNode = closedNodes[i];
            }
        }

        while (!currentNode.Equals(to))
        {
            currentNode = currentNode.GetParent();
            path.Add(currentNode);
        }

        foreach (PathNode waypoint in path)
        {
            Debug.Log("WAYPOINT " + waypoint);
        }

        return path;

    }

}
