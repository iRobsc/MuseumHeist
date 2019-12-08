using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class pathy : MonoBehaviour
{


    /* Data structures */
    public class Node {
        public int x, y;
        public float cost;

        public Node(int x, int y, float cost) {
            this.x = x;
            this.y = y;
            this.cost = cost;
        }

        public static bool operator ==(Node a, Node b) {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Node a, Node b) {
            return a.x != b.x || a.y != b.y;
        }
    }

    /* public stuff */
    public Tilemap tilemap;
    public TileBase wallTile;
    public TileBase markingTile;
    public TileBase markingTile_type2;

    private float heuristic(Node from, Node to) {
        float dx = to.x - from.x;
        float dy = to.y - from.y;
        return Mathf.Sqrt(dx*dx + dy*dy);
    }

    private void calculate_neighbours(Node parent, Node end, List<Node> open, List<Node> closed, Dictionary<Node,Node> came_from) {
        float diag = 3.0f*1.4142f;
        float straight = 1.0f;
        for (int dy = -1; dy <= 1; dy++) {
            for (int dx = -1; dx <= 1; dx++) {
                if (dx == 0 && dy == 0)
                    continue;
                float stepcost = (Mathf.Abs(dx) + Mathf.Abs(dy) < 2.0f) ? straight : diag;
                int x = parent.x + dx;
                int y = parent.y + dy;
                Node neighbour = new Node(x, y, -1.0f);

                // check if node is closed 
                bool closed_contains = false;
                foreach (Node n in closed) {
                    if (neighbour == n) {
                        closed_contains = true;
                        break;
                    }
                }
                if (closed_contains)
                    continue;

                // check if node is walltile
                TileBase tile = tilemap.GetTile(new Vector3Int(neighbour.x, neighbour.y, 0));
                if (tile == wallTile)
                    continue;

                // if node is not in open add it.
                neighbour.cost = stepcost + heuristic(neighbour, end);
                bool in_open = false;
                foreach (Node n in open) {
                    if (neighbour == n) {
                        in_open = true;
                        break;
                    }
                }
                if (!in_open) {
                    open.Add(neighbour);
                    came_from.Add(neighbour, parent);
                }
            }
        }
    }

    public List<Node> find_path(GameObject from, GameObject to) {
        Dictionary<Node, Node> came_from = new Dictionary<Node,Node>();
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        Node start = new Node((int)Mathf.Round(from.transform.position.x), (int)Mathf.Round(from.transform.position.y), -1.0f);
        Node end = new Node((int)Mathf.Round(to.transform.position.x), (int)Mathf.Round(to.transform.position.y), -1.0f);
        
        start.cost = heuristic(start, end);
        open.Add(start);

        while (open.Count > 0) {
            // choose lowest cost in open
            Node current = null;
            float currentF = 0.0f;
            foreach (Node node in open) {
                if (current is null || node.cost < currentF) {
                    current = node;
                    currentF = node.cost;
                }
            }

            // if current is end retrace path
            if (current == end) {
                print("retrace path");
                List<Node> path = new List<Node>();
                while(came_from.ContainsKey(current)) {
                    path.Add(current);
                    // debug print
                    //tilemap.SetTile(new Vector3Int(current.x, current.y, 0), markingTile_type2);
                    current = came_from[current];
                }
                path.Reverse();
                return path;
            }

            // move from open to closed
            open.Remove(current);
            closed.Add(current);

            // add neighbours to open
            calculate_neighbours(current, end, open, closed, came_from);
        }

        print("no path found");
        return null;
    }
    
}    
