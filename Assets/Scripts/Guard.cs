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

    private Animator animator;

    private int pathIndex;

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

        animator = this.GetComponent<Animator>();

        pathIndex = 0;

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
            speed = 0.05f;
            
            AStar astar = this.GetComponent<AStar>();
            List<PathNode> pathpoints = astar.FindPath(this.gameObject, playerObject);
            Debug.Log("GUARD " + this.gameObject.transform.position);
            Debug.Log("PLAYER " + playerObject.transform.position);
            Debug.Log("TARGET " + target.transform.position);
            Debug.Log("COUNT " + pathpoints.Count);
            foreach (PathNode pathnode in pathpoints)
            {
                Debug.Log(pathnode);
            }
            Debug.Log("FIRST " + pathpoints[0]);
            Debug.Log("SECOND " + pathpoints[1]);
            Debug.Log("LAST " + pathpoints[pathpoints.Count - 1]);
            Debug.Log("CONDITION " + (distance(this.transform.position, new Vector2(pathpoints[0].x, pathpoints[0].y)) < 0.1));
            if (distance(this.transform.position, new Vector2(pathpoints[0].x, pathpoints[0].y)) < 0.1) {
                pathIndex++;
            }
            target.transform.position = new Vector3(pathpoints[pathIndex].x, pathpoints[pathIndex].y, 0);

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

        //animation
        float currX = transform.position.x;
        float currY = transform.position.y;

        float targetX = target.transform.position.x;
        float targetY = target.transform.position.y;

        if (Mathf.Abs(targetX - currX) < 1.0f)
        {
            if (currY < targetY)
            {
                animator.SetBool("downwards", false);
                animator.SetBool("left", false);
                animator.SetBool("right", false);
                animator.SetBool("upwards", true);
            }
            else
            {
                animator.SetBool("upwards", false);
                animator.SetBool("left", false);
                animator.SetBool("right", false);
                animator.SetBool("downwards", true);
            }
        }
        else {
            if (currX < targetX)
            {
                animator.SetBool("upwards", false);
                animator.SetBool("downwards", false);
                animator.SetBool("left", false);
                animator.SetBool("right", true);
            }
            else
            {
                animator.SetBool("upwards", false);
                animator.SetBool("downwards", false);
                animator.SetBool("right", false);
                animator.SetBool("left", true);
            }
        }

    }
}
