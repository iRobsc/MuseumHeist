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
    public float fov_dot_thresh = 0.6f;
    public float player_visibility_thresh = 0.05f;
    public float player_audibility_radius_thresh = 3.0f;
    private GameObject playerObject;
    
    private GameObject target;
    private GameObject[] waypoints;
    private int waypointIndex;

    private Animator animator;

    private int pathpointIndex;
    public Transform pathfinding;
    private GameObject[] pathpoints;
    AStar aStar;
    private bool foundClosestPathpoint;
    private bool reached;
    private bool pathpointsCalculated;
    private Vector2 last_pos = new Vector2(0.0f, 0.0f);
    Vector2 walkDirection = new Vector2(0.0f, 0.0f);

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

        foundClosestPathpoint = false;
        reached = false;
        pathpointIndex = 0;
        pathpointsCalculated = false;
        aStar = this.GetComponent<AStar>();
        //calculatePathpoints();
    }

    private GameObject temp_target = null;
    private bool investigate_noise = false;
    public void notify_of_noise(Vector2 position, float sound_level) {
        // TODO react to noise if close and loud enough  
        /*print(this + " notified of noise at " + position);
        investigate_noise = true;
        temp_target = new GameObject();
        temp_target.transform.position = (Vector3) position;
        //Instantiate(temp_target, position);
        print(temp_target.transform.position);
        target = temp_target;*/ 
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
    int cc = 0;
    bool standing_still = false;
    int standing_still_counter = 0;
    void Update()
    {

        if (standing_still) {
            standing_still = (++standing_still_counter % 300) != 0;
            animator.enabled = false;
            if (!standing_still)
                animator.enabled = true;
            return;
        }

        if (investigate_noise) {
           calculatePathpoints(); 
        }

        if (!detected && pathfinding.childCount > 0)
        {
            deletePathpoints();
        }

        if (detected)
        {
            if (!pathpointsCalculated)
            {
                print("calc pp");
                if (!calculatePathpoints()) {
                    target = playerObject;
                };
                //pathpointsCalculated = true;
                if (pathpoints.Length < 3){
                    target = playerObject;
                    print("target : player");
                    speed = 0.08f;
                } else {
                    target = pathpoints[2];
                    speed = 0.06f;
                    print("target idx 1");
                }
            }

            //checking if on the way to or back from the target
            if(target == null)
                return;
            if (distance(this.transform.position, target.transform.position) < 0.1)
            {
                print(pathpointIndex);
                pathpointIndex++;

                if (pathpointIndex > pathpoints.Length - 1) {
                    Debug.Log("IT'S OVER");
                    reached = false;
                    pathpointsCalculated = false;
                    foundClosestPathpoint = false;
                    deletePathpoints();
                    return;
                }

            }

            if(pathpoints.Length > pathpointIndex)
                target = pathpoints[pathpointIndex];

        }
        else {
            target = waypoints[waypointIndex];
            Vector2 ray = direction(transform.position, target.transform.position);
            LayerMask collidables = LayerMask.GetMask("Collidables");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -ray, collidables);
            float dx = transform.position.x - target.transform.position.x;
            float dy = transform.position.y - target.transform.position.y;
            float distance = Mathf.Sqrt(dx*dx+dy*dy);
            if (hit != null && hit.collider.name == "door") {
                if (distance > hit.distance && (hit.transform.GetChild(0).GetComponent<DoorBlocker>().is_blocking)) {
                    standing_still = true;
                    for (int i = waypointIndex; i < waypoints.Length; i++) {
                        Vector2 ray1 = direction(transform.position, waypoints[i].transform.position);
                        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, -ray1, collidables);
                        float dx1 = transform.position.x - waypoints[i].transform.position.x;
                        float dy1 = transform.position.y - waypoints[i].transform.position.y;
                        float distance1 = Mathf.Sqrt(dx1*dx1+dy1*dy1);
                        if (hit1 == null){
                            waypointIndex = i;
                            break;
                        } else if (hit1.distance > distance1) {
                            waypointIndex = i;
                            break;
                        }

                    }
                }
            }
        }

        if (target == waypoints[waypointIndex] && distance(transform.position, target.transform.position) < 0.3f)
        {
            waypointIndex ++;
            if (waypointIndex == waypoints.Length)
                waypointIndex = 0;
            target = waypoints[waypointIndex];
        } 
        else if (!detected) 
        {
            Vector2 view_direction = walkDirection; 
            Vector2 ray = direction(transform.position, playerObject.transform.position);
            LayerMask collidables = LayerMask.GetMask("Collidables");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -ray, collidables);
            bool in_fov = Vector2.Dot(view_direction, ray) > fov_dot_thresh;
            bool visible = playerObject.GetComponent<Movement>().playerNoiseAndVisibility.visibility > player_visibility_thresh;
            visible = playerObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled ? true : visible;
            if (
                    hit.transform != null && 
                    hit.collider == player_collider && 
                    in_fov &&
                    visible
            ) {
                print("player spotted" + playerObject.GetComponent<Movement>().playerNoiseAndVisibility.visibility);
                detected = true;
                return;
            }

            /*float dx = playerObject.transform.position.x - transform.position.x;
            float dy = playerObject.transform.position.y - transform.position.y;
            float pg_distance = Mathf.Sqrt(dx*dx+dy*dy);
            if (pg_distance < player_audibility_radius_thresh * playerObject.GetComponent<Movement>().playerNoiseAndVisibility.noise) {
                print("player heard");
                detected = true;
                return;
            }*/
        }
        
        walkDirection = direction(transform.position, target.transform.position);
        float posX = transform.position.x - speed * walkDirection.x;
        float posY = transform.position.y - speed * walkDirection.y;

        //last_pos = (Vector2) transform.position;
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

    void deletePathpoints() {
        pathfinding.DetachChildren();
        for (int i = 0; i < pathpoints.Length; i++)
        {
            Destroy(GameObject.Find("pathpoint " + i));
        }
    }

    bool calculatePathpoints() {
        List<PathNode> aStarPoints = aStar.FindPath(this.gameObject, playerObject);
        if (aStarPoints == null) {
            print("recalculating path!");
            pathpointsCalculated = false;
            return false;
        }

        for (int i = 0; i < aStarPoints.Count; i++)
        {
            GameObject go = new GameObject();
            go.transform.parent = pathfinding;
            go.name = "pathpoint " + i;
            go.transform.position = new Vector3(aStarPoints[i].x + 1, aStarPoints[i].y + 1, 0);
        }
        int pathpointAmount = pathfinding.childCount;
        pathpoints = new GameObject[pathpointAmount];
        for (int i = 0; i < pathpointAmount; i++)
        {
            pathpoints[i] = pathfinding.GetChild(i).gameObject;
        }
        pathpointsCalculated = true;
        return true;
        //pathpointIndex = 0;
    }
}
