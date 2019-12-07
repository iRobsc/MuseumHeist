﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorBlocker : MonoBehaviour
{

    /* public stuff */
    public bool is_blocking = false;
    public GameObject blocking_group;
    public Tilemap tileMap;
    public Vector3Int blocktile1;
    public Vector3Int blocktile2;
    public TileBase blockingtile;

    /* private stuff */
    private List<Collider2D> blocking_colliders;
    private int n_is_blocking = 0;

    void OnTriggerEnter2D(Collider2D collider)
    {
        foreach (Collider2D c in blocking_colliders) {
            if (c == collider) {
                n_is_blocking++;
                if (n_is_blocking == 1) {
                    tileMap.SetTile(blocktile1, blockingtile);
                    tileMap.SetTile(blocktile2, blockingtile);
                }
                break;
            }
        }
        this.is_blocking = n_is_blocking > 0;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        foreach (Collider2D c in blocking_colliders) {
            if (c == collider) {
                n_is_blocking--;
                if (n_is_blocking == 0) {
                    tileMap.SetTile(blocktile1, null);
                    tileMap.SetTile(blocktile2, null);
                }
                break;
            }
        }
        this.is_blocking = n_is_blocking > 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        blocking_colliders = new List<Collider2D>();
        foreach (Transform child in blocking_group.transform) {
            blocking_colliders.Add(child.GetComponent<Collider2D>());
        }
    }

}
