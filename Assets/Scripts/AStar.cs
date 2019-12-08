using System;
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
    public TileBase markingTile;
    public TileBase markingTile_type2;

    private Vector3Int size;
    private BoundsInt bounds;

    private void Awake()
    {
        size = tilemap.size;
        bounds = tilemap.cellBounds;
    }
    
    public List<PathNode> FindPath(GameObject chaser, GameObject target)
    {
        Vector3 chaserPos = chaser.transform.position;
        Vector3 targetPos = target.transform.position;

        startingNode = new PathNode((int)Mathf.Round(chaserPos.x), (int)Mathf.Round(chaserPos.y));
        targetNode = new PathNode((int)Mathf.Round(targetPos.x), (int)Mathf.Round(targetPos.y));
        
        TileBase starttile = tilemap.GetTile(new Vector3Int(startingNode.x, startingNode.y, 0));
        TileBase targettile = tilemap.GetTile(new Vector3Int(targetNode.x, targetNode.y, 0));
        if (startingNode == wallTile || targettile == wallTile)
            return null;
        //decomment to debug
        //tilemap.SetTile(new Vector3Int(startingNode.x, startingNode.y, 0), markingTile);
        //tilemap.SetTile(new Vector3Int(targetNode.x, targetNode.y, 0), markingTile);

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
                if (!(tile == wallTile))
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
        try {
            List<PathNode> path = new List<PathNode>();
            PathNode currentNode = new PathNode(0, 0);

            for (int i = 0; i < closedNodes.Count; i++)
            {
                if (closedNodes[i].x == from.x && closedNodes[i].y == from.y)
                {
                    currentNode = closedNodes[i];
                    break;
                }
            }

            while (!currentNode.Equals(to))
            {
                currentNode = currentNode.GetParent();
                path.Add(currentNode);
            }
            path.Reverse();

            //decomment to debug
            foreach (PathNode waypoint in path)
            {
                //Debug.Log("WAYPOINT " + waypoint);
                tilemap.SetTile(new Vector3Int(waypoint.x, waypoint.y, 0), markingTile_type2);
            }

            return path;
        } catch (Exception e) {
            return null;
        }
    }

}
