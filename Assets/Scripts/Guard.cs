﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guard : MonoBehaviour
{
    public Collider2D player_collider;
    public GameObject[] waypoints;
    public GameObject affectedByGroup;
    public float speed;
    public string next_scene;
    public bool detected;
    private GameObject playerObject;
    
    private GameObject target;
    private int waypointIndex;
    

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        playerObject = player_collider.gameObject;
        detected = false;
        target = playerObject;
    }

    /*
     * Switch to next scene if level is completed
     */
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider != player_collider)
            return;
        SceneManager.LoadScene(next_scene);
    }

    // Update is called once per frame
    void Update()
    {
        if (detected)
        {
            target = playerObject;
            speed = 0.02f;
        }
        else
            target = waypoints[waypointIndex];

        
        float deltaX = transform.position.x - target.transform.position.x;
        float deltaY = transform.position.y - target.transform.position.y;
        float angleX = (float)Math.Cos(Math.Atan2(deltaY, deltaX));
        float angleY = (float)Math.Sin(Math.Atan2(deltaY, deltaX));

        if (target == waypoints[waypointIndex] && Math.Abs(deltaY) < 1 && Math.Abs(deltaX) < 1)
        {
            waypointIndex ++;
            if (waypointIndex == waypoints.Length)
                waypointIndex = 0;
            target = waypoints[waypointIndex];
        } 
        else if (!detected && affectedByGroup.activeSelf) 
        {
            LayerMask playerLayer = LayerMask.GetMask("Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(-angleX, -angleY), 10, playerLayer);
            if (hit.transform != null && hit.transform.gameObject == player_collider.gameObject)
                detected = true;
        }
        
        float posX = transform.position.x - speed * angleX;
        float posY = transform.position.y - speed * angleY;
        transform.position = new Vector3(posX, posY, 0);
    }
}