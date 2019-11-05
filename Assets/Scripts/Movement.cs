using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    /* public stuff */
    public float acceleration_coefficient;
    public float drag_coefficient;
    public float max_velocity;
    public float shift_multiplier;


    /* private stuff */
    private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {

        // apply drag to velocity
        this.velocity.x *= 1 - drag_coefficient;
        this.velocity.y *= 1 - drag_coefficient;

        // apply velocity to position
        transform.position += this.velocity;
        
        // translate input to player movement
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0f);
        float dv_x = input.x * Time.deltaTime * this.acceleration_coefficient;
        float dv_y = input.y * Time.deltaTime * this.acceleration_coefficient;

        // apply change to velocity
        bool shift_down = Input.GetKey(KeyCode.LeftShift);
        dv_x *= shift_down ? shift_multiplier : 1.0f;
        dv_y *= shift_down ? shift_multiplier : 1.0f;
        this.velocity.x += (Mathf.Abs(dv_x + this.velocity.x) > max_velocity) ? 0 : dv_x;
        this.velocity.y += (Mathf.Abs(dv_y + this.velocity.y) > max_velocity) ? 0 : dv_y;
    }
}
