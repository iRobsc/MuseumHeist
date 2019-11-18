using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Guard : MonoBehaviour
{
    public Collider2D player_collider;
    public Transform waypointsObject;
    public GameObject roomLights;
    public float speed;
    public float sightRange;
    public string next_scene;
    public bool detected;
    private GameObject playerObject;
    
    private GameObject target;
    private GameObject[] waypoints;
    private int waypointIndex;
    

    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        playerObject = player_collider.gameObject;
        detected = false;
        target = playerObject;
        if (sightRange == 0)
            sightRange = 10;

        int waypointAmount = waypointsObject.childCount;
        waypoints = new GameObject[waypointAmount];
        for (int i = 0; i < waypointAmount; i++)
        {
            waypoints[i] = waypointsObject.GetChild(i).gameObject;
        }
    }

    public void notify_of_noise(Vector2 position, float sound_level) {
        // TODO react to noise if close and loud enough  
        print(this + " notified of noise at " + position);
    }

    private double distance(Vector2 from, Vector2 to) {
        float deltaX = from.x - to.x;
        float deltaY = from.y - to.y;
        return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
    }

    private Vector2 direction(Vector2 from, Vector2 to) {
        float deltaX = from.x - to.x;
        float deltaY = from.y - to.y;
        float angleX = (float)Math.Cos(Math.Atan2(deltaY, deltaX));
        float angleY = (float)Math.Sin(Math.Atan2(deltaY, deltaX));

        return new Vector2(angleX, angleY);
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
            speed = 0.05f;
        }
        else
            target = waypoints[waypointIndex];

        if (target == waypoints[waypointIndex] && distance(transform.position, target.transform.position) < 1)
        {
            waypointIndex ++;
            if (waypointIndex == waypoints.Length)
                waypointIndex = 0;
            target = waypoints[waypointIndex];
        } 
        else if (!detected && roomLights.activeSelf) 
        {
            Vector2 sightDirection = direction(transform.position, playerObject.transform.position);
            LayerMask collidables = LayerMask.GetMask("Collidables");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -sightDirection, sightRange, collidables);
            if (hit.transform == null && distance(transform.position, playerObject.transform.position) <= sightRange)
            {
                print(hit.collider);
                detected = true;
            }
                
        }
        
        Vector2 walkDirection = direction(transform.position, target.transform.position);
        float posX = transform.position.x - speed * walkDirection.x;
        float posY = transform.position.y - speed * walkDirection.y;
        transform.position = new Vector3(posX, posY, 0);
    }
}
