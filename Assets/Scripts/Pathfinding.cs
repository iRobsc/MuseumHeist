using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node {

    public int x; //x position
    public int y; //y position
    public int g_cost; //distance from start node
    public int h_cost; //distance from end node
    public int f_cost; //g + h cost

    public Node parent;

    public Node(int x, int y) {

        this.x = x;
        this.y = y;

    }

    override
    public System.String ToString() {
        return "Position: [" + this.x + ", " + this.y + "]\n" +
            "Costs: [G(" + g_cost + "), H("+ h_cost + "), F(" + GetFCost() + ")]";
    }

    public int GetFCost() {
        return this.g_cost + this.h_cost;
    }

    public int GetDistanceTo(Node to)
    {
        Debug.Log("GET DISTANCE TO");
        Debug.Log(to.ToString());
        Debug.Log(this.ToString());

        int distX = Mathf.Abs(this.x - to.x);
        int distY = Mathf.Abs(this.y - to.y);

        if (distX > distY)
        {
            Debug.Log("DISTANCE " + (14 * distY + 10 * (distX - distY)));

            return 14 * distY + 10 * (distX - distY);
        }
        else
        {
            Debug.Log("DISTANCE " + (14 * distX + 10 * (distY - distY)));

            return 14 * distX + 10 * (distY - distY);
        }
    }

    public bool isDiagonalTo(Node to)
    {

        bool xChanged = !(this.x == to.x);
        bool yChanged = !(this.y == to.y);

        return xChanged && yChanged;

    }

}

public class Pathfinding : MonoBehaviour
{

    public Grid grid;
    public Tilemap tilemap;

    public TileBase wall;

    public TileBase thief;
    private Node targetNode;

    public TileBase bottomLeftMarker;
    private Vector2Int bottomLeftMarkerPos;

    public TileBase topRightMarker;
    private Vector2Int topRightMarkerPos;

    private List<Node> openNodes;
    private HashSet<Node> closedNodes;

    BoundsInt bounds;
    Vector3Int size;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ONE");

        openNodes = new List<Node>();
        closedNodes = new HashSet<Node>();

        bounds = tilemap.cellBounds;
        size = tilemap.size;

        /*for (int i = bounds.x; i < size.x; i++)
        {
            for (int j = bounds.y; j < size.y; j++)
            {
                for (int k = bounds.z; k < size.z; k++)
                {
                    if (tilemap.HasTile(new Vector3Int(i, j, k)))
                    {

                        TileBase tile = tilemap.GetTile(new Vector3Int(i, j, k));

                        if (tile.Equals(bottomLeftMarker))
                        {
                            bottomLeftMarkerPos = new Vector2Int(i, j);
                        }

                        if (tile.Equals(topRightMarker))
                        {
                            topRightMarkerPos = new Vector2Int(i, j);
                        }

                        if (tile.Equals(thief))
                        {
                            targetNode = new Node(i, j);
                        }


                    }
                }
            }
        }*/

        FindPath();

    }
    void FindPath()
    {
        //Debug.Log("TWO");
        bool pathFound = false;

        Node startingNode = new Node(0, 0);
        targetNode = new Node(1, 1);

        openNodes.Add(startingNode);
        Node currentNode = openNodes[0];

        List<Node> neighbors;

        while (!pathFound)
        {

            //Debug.Log("CURRENT NODE");
            //Debug.Log(currentNode.ToString());

            for (int i = 1; i < openNodes.Count; i++)
            {
                Node newNode = openNodes[i];
                if (newNode.f_cost < currentNode.f_cost || (newNode.f_cost == currentNode.f_cost && newNode.h_cost < currentNode.h_cost))
                {
                    currentNode = openNodes[i];
                }
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathFound = true;
            }

            neighbors = GetNeighbors(currentNode);

            /*foreach (Node neighbor in neighbors)
            {
                Debug.Log("NEIGHBOR");
                Debug.Log(neighbor.ToString());

                if (tilemap.HasTile(new Vector3Int(neighbor.x, neighbor.y, 0)) || closedNodes.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.g_cost + currentNode.GetDistanceTo(neighbor);
                if (newMovementCostToNeighbor < neighbor.g_cost || !openNodes.Contains(neighbor))
                {
                    neighbor.g_cost = newMovementCostToNeighbor;
                    neighbor.h_cost = neighbor.GetDistanceTo(targetNode);
                    neighbor.parent = currentNode;

                    Debug.Log("NEIGHBOR");
                    Debug.Log(neighbor.ToString());

                    if (!openNodes.Contains(neighbor)) {
                        openNodes.Add(neighbor);
                    }
                }

            }*/

        }
    }

    List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

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

                if (checkX > bounds.x && checkX < size.x && checkY > bounds.y && checkY < size.y)
                {
                    Node neighbor = new Node(checkX, checkY);
                    neighbors.Add(neighbor);
                }
            }
        }
        
        return neighbors;
    }

    void RetracePath(Node from, Node to) {
        List<Node> path = new List<Node>();
        Node currentNode = to;

        while (currentNode != from) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        foreach (Node point in path)
        {
            Debug.Log("WAYPOINT " + point);
        }
        
    }

}
