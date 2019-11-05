using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    /* public stuff */
    public GameObject player;
    public Camera camera;
    public float distance_threshold;
    public float centering_speed;
    public float distance_offset;

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosition = camera.transform.InverseTransformDirection(
            player.transform.position - camera.transform.position
        );
        float d_x = relativePosition.x;
        float d_y = relativePosition.y;
        float d = Mathf.Sqrt(d_x*d_x + d_y*d_y);
        float dir = (Mathf.Atan2(d_y, d_x) + 2*Mathf.PI) % (2*Mathf.PI);
        float d_distance = d - distance_threshold;
        Vector3 move_dir = new Vector3(
            Mathf.Cos(dir),
            Mathf.Sin(dir),
            0.0f
        );

        // keep player within a certain radious of camera center
        if (d > distance_threshold) {
            transform.position += move_dir * d_distance;
        }
        
        // slowly center camera
        transform.position += (d + distance_offset) * move_dir * centering_speed;
    }
}
