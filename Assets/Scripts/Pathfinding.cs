using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{

    public Grid grid;
    public Tilemap tilemap;

    BoundsInt bounds;
    Vector3Int size;

    void Awake()
    {
        size = tilemap.size;
    }

    void Update()
    {
        List<Object> openNodes = new List<Object>();
        HashSet<PathNode> closedNodes = new HashSet<PathNode>();

        PathNode startingNode = new PathNode(0, 0);
        PathNode targetNode = new PathNode(10, 10);

        Debug.Log("STARTING NODE " + startingNode);
        Debug.Log("TARGET NODE " + targetNode);
        Debug.Log(startingNode.x == targetNode.x && startingNode.y == targetNode.y);

        openNodes.Add(startingNode);

        List<Object> neighbors;

        while (openNodes.Count > 0)
        {
            Debug.Log("1- Houston, We're In!!");
            PathNode currentNode = (PathNode) openNodes[0];

            Debug.Log("CURRENT NODE " + currentNode);
            Debug.Log("TARGET NODE " + targetNode);
            Debug.Log(currentNode.x == targetNode.x && currentNode.y == targetNode.y);


            for (int i = 1; i < openNodes.Count; i++)
            {
                Debug.Log("2- Comparing Nodes Now");
                PathNode newNode = (PathNode) openNodes[i];
                if (newNode.GetFCost() < currentNode.GetFCost() || (newNode.GetFCost() == currentNode.GetFCost() && newNode.h_cost < currentNode.h_cost))
                {
                    Debug.Log("2.1- We have a case");
                    currentNode = newNode;
                }
            }

            if (currentNode.x == targetNode.x && currentNode.y == targetNode.y)
            {
                Debug.Log("FINAL- We got it!");
                return;
            }

            Debug.Log("3- removing and adding");
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            Debug.Log("LIST PROPS: " + openNodes.Count + " " + closedNodes.Count);

            Debug.Log("4- Getting Neighbors");
            neighbors = GetNeighbors(currentNode);

            for (int i = 0; i < neighbors.Count; i++)
            {
                PathNode neighbor = (PathNode) neighbors[i];
                Debug.Log("4.1- for each neighbor");
                /*if (tilemap.HasTile(new Vector3Int(neighbor.x, neighbor.y, 0)) || closedNodes.Contains(neighbor))
                {
                    continue;
                }*/

                int newMovementCostToNeighbor = currentNode.g_cost + currentNode.GetDistanceTo(neighbor);
                if (newMovementCostToNeighbor < neighbor.g_cost || !ListContainsPathNode(openNodes, neighbor))
                {
                    Debug.Log("4.2- comparing G_Cost");
                    neighbor.g_cost = newMovementCostToNeighbor;
                    neighbor.h_cost = neighbor.GetDistanceTo(targetNode);
                    neighbor.parent = currentNode;
                    
                    if (!ListContainsPathNode(openNodes, neighbor)) 
                    {
                        Debug.Log("4.3- adding neighbor to openNodes");
                        openNodes.Add(neighbor);
                        Debug.Log("LIST PROPS: " + openNodes.Count + " " + closedNodes.Count);
                    }
                }

            }

        }

        Debug.Log("5- retracing path");
        RetracePath(targetNode, startingNode);
    }

    List<Object> GetNeighbors(PathNode node)
    {
        List<Object> neighbors = new List<Object>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int checkX = node.x + i;
                int checkY = node.y + j;
                Debug.Log(checkX);
                Debug.Log(checkY);
                Debug.Log("------------------------------");
                Debug.Log(checkX > 0 && checkX < size.x && checkY > 0 && checkY < size.y);

                if (checkX > 0  && checkX < size.x && checkY > 0 && checkY < size.y)
                {
                    PathNode neighbor = new PathNode(checkX, checkY);
                    neighbors.Add(neighbor);
                }
            }
        }
        
        return neighbors;
    }

    void RetracePath(PathNode from, PathNode to) {
        
        List<Object> path = new List<Object>();
        PathNode currentNode = to;

        while (currentNode.x != from.x && currentNode.y != from.y) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Add(currentNode);
        currentNode = currentNode.parent;

        path.Reverse();

        if (path.Count <= 0) {
            return;
        }

        foreach (PathNode waypoint in path)
        {
            Debug.Log("WAYPOINT " + waypoint.ToString());
        }
    
    }

    bool ListContainsPathNode(List<Object> list, PathNode node) {
        foreach (PathNode pathnode in list) {
            if (pathnode.x == node.x && pathnode.y == node.y) {
                return true;
            }
        }
        return false;
    }

}
