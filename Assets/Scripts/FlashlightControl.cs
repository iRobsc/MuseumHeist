using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightControl : MonoBehaviour
{

    /* public stuff */
    public GameObject flashlight;
    public Camera camera;
    public float flashlight_distance_from_player;

    /* private stuff */
    private const float rad_to_deg = 57.2957795f;

    // Update is called once per frame
    void Update()
    {
        // update rotation
        Vector3 player_pos = camera.WorldToScreenPoint(transform.position);
        Vector3 mouse_pos = Input.mousePosition;
        float d_x = mouse_pos.x - player_pos.x;
        float d_y = mouse_pos.y - player_pos.y;
        float angle = (Mathf.Atan2(d_y, d_x) * rad_to_deg + 360) % 360;
        flashlight.transform.eulerAngles = new Vector3 (
            0.0f,
            0.0f,
            angle - 90
        );

        // update position (offset from player)
        flashlight.transform.position = transform.position + new Vector3(
            flashlight_distance_from_player * Mathf.Cos(angle / rad_to_deg),
            flashlight_distance_from_player * Mathf.Sin(angle / rad_to_deg),
            0.0f
        ); 

        if (Input.GetMouseButtonDown(0) /* 0 for left click, 1 for right click*/) {
            flashlight.GetComponent<MeshRenderer>().enabled = 
                    (flashlight.GetComponent<MeshRenderer>().enabled) ? false : true;
        } 
    }
}
