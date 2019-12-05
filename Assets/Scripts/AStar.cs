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
    public TileBase wallTile;

    public GameObject target;
    public GameObject chaser;

    private Vector3Int size;
    private BoundsInt bounds;

    private void Awake()
    {
        size = tilemap.size;
        bounds = tilemap.cellBounds;
    }

    public void Update()
    {
        Vector3 chaserPos = chaser.transform.position;
        Vector3 targetPos = target.transform.position;

        startingNode = new PathNode((int)chaserPos.x, (int)chaserPos.y);
        targetNode = new PathNode((int)targetPos.x, (int)targetPos.y);

        FindPath(startingNode, targetNode);
    }

    public List<PathNode> FindPath(PathNode startingNode, PathNode targetNode)
    {

        openNodes = new List<PathNode>();
        closedNodes = new List<PathNode>();

        openNodes.Add(startingNode);
        PathNode currentNode = openNodes[0];

        while (!targetNode.Equals(currentNode) && openNodes.Count > 0)
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
                {                   
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

                TileBase tile = tilemap.GetTile(new Vector3Int(neighbourX, neighbourY, 0));
                if (tile == null)
                {
                    if (neighbourX >= bounds.x && neighbourX < size.x && neighbourY >= bounds.y && neighbourY < size.y)
                    {
                        PathNode neighbour = new PathNode(neighbourX, neighbourY);
                        neighbour.g_cost = neighbour.GetDistanceTo(startingNode);
                        neighbour.h_cost = neighbour.GetDistanceTo(targetNode);
                        neighbour.GetFCost();
                        neighbours.Add(neighbour);
                    }
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

        Debug.Log("---------------");
        foreach (PathNode waypoint in path)
        {
            Debug.Log("WAYPOINT " + waypoint);
            tilemap.SetTile(new Vector3Int(waypoint.x, waypoint.y, 0), wallTile);
        }
        foreach (PathNode waypoint in path)
        {
            tilemap.SetTile(new Vector3Int(waypoint.x, waypoint.y, 0), null);
        }
        Debug.Log(path.Count);
        Debug.Log("---------------");

        return path;

    }

}
